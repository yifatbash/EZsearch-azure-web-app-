var searchesList = [];
var search = {

    saveToDB: function (searchValue) {
        if (searchValue === '') {
            alert("Please enter a value to search..");
            return;
        }

        var date = new Date();

        var requestBody = {
            value: searchValue,
            date: date
        }

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: '/api/search/saveToHistory',
            data: JSON.stringify(requestBody),
            success: function () {
                $('#search-block').val("");
            },
            error: function (request, status, error) {
                console.log(JSON.parse(request.response).ExceptionMessage);
            }
        })
    },

    //init the dropdown with the searches history from DB
    initHistoryList: function () {
        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: '/api/search/getHistoryList',
            dataType: "json",
            success: function (data) {
                var historyList = JSON.parse(data);
                searchesList = historyList.map(function (item) {
                    return item['Value'];
                });
                $('#search-block').autocomplete({
                    source: searchesList,
                    minLength: 0,
                    scroll: true,
                }).focus(function () {
                    $(this).autocomplete("search", "");
                });
            },
            error: function (request, status, error) {
                alert(JSON.parse(request.response).ExceptionMessage);
            }
        });

    },

    //call to the bing-web-search-api
    callBing: function () {
        if ($('#search-block').val() === '') {
            alert("Please enter a value to search..");
            return;
        }
        var searchQuery = $('#search-block').val();

        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: '/api/search/bing',
            dataType: "json",
            data: { "searchQuery": searchQuery },
            success: function (data, status) {
                var searchValue = $('#search-block').val();
                $('#search-block').val(""); //set the textbox to be empty

                $('.results').empty(); //clean the previous results
                
                var js = JSON.parse(data);

                for (var i = 0; i < js.webPages.value.length; i++)
                {
                    var url = js.webPages.value[i].url;
                    var name = js.webPages.value[i].name;
                    var displayUrl = js.webPages.value[i].displayUrl;
                    var snippet = js.webPages.value[i].snippet;

                    //enter the new results into the page
                    $('.results').append("<div class=\"result-form\"><a href=\"" + url + "\" target=\"_blank\">" + name + "</a>");
                    $('.results').append("<div class=\"green-webpages-url\">" + displayUrl + "</div>");
                    $('.results').append("<div>"+snippet+"</div></div>");
                }
                search.saveToDB(searchValue);

                //update the dropdown
                var index = searchesList.indexOf(searchValue);
                if (index !== -1)
                    searchesList.splice(index, 1);
                searchesList.unshift(searchValue);
                $("#search-block").autocomplete("option", "source", searchesList);
            },
            error: function (request, status, error) {
                alert(JSON.parse(request.response).ExceptionMessage);
            }
        });
    }
}