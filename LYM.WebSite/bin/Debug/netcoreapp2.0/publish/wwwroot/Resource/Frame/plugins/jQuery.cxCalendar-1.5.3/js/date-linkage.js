function dateLinkage(startSelector, endSelector) {
    var dateFirst = $(startSelector);
    var dateLast = $(endSelector);
    var dateFirstApi;
    var dateLastApi;

    dateFirst.cxCalendar(function(api) {
        dateFirstApi = api;
    });

    dateLast.cxCalendar(function(api) {
        dateLastApi = api;
    });

    dateFirst.bind('change', function() {
        var firstTime = parseInt(dateFirstApi.getDate('TIME'), 10);
        var lastTime = parseInt(dateLastApi.getDate('TIME'), 10);

        if (lastTime < firstTime) {
            dateLastApi.clearDate();
        };

        dateLastApi.setOptions({
            startDate: firstTime
        });
        dateLastApi.gotoDate(firstTime);
        dateLastApi.show();
    });
}