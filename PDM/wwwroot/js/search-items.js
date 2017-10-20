$(document).ready(function () {
    $("#srcInternalCode, #srcMachineType, #srcItemType, #srcEnter, #srcExit,#srcThickness,#srcBand,#srcElastic,#srcPdm,#srcStatus").keyup(function () {
        var getFilter = function (el, selector) {
            var txt = $(el).val().toLowerCase();
            return txt
                ? function (i, p) { return $(p).find(selector).text().toLowerCase().indexOf(txt) !== -1; }
                : function (i, p) { return true; };
        };

        $('.panel-info').hide()
            .filter(getFilter('#srcInternalCode', '.panel-heading'))
            .filter(getFilter('#srcMachineType', '#types'))
            .filter(getFilter('#srcItemType', '#types'))
            .filter(getFilter('#srcEnter', '#specs'))
            .filter(getFilter('#srcExit', '#specs'))
            .filter(getFilter('#srcThickness', '#specs'))
            .filter(getFilter('#srcBand', '#specs'))
            .filter(getFilter('#srcElastic', '#specs'))
            .filter(getFilter('#srcPdm', '#specs'))
            .filter(getFilter('#srcStatus', '#types'))
            .show();
    });
});