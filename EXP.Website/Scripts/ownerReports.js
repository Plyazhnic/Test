function OwnerReports(commonFunctionality) {
    var self = this;
    this.common = commonFunctionality;
    this.cacheEnable = false;
    this.locCommon = new Common();

    this.DisplayBuilding = function () {
        $("#reportsOwnerBuildingName").text($("#select-building option:selected").text());

        var buildingId = $("#select-building option:selected").val();
        var days = $("#search-days option:selected").val();

        $.ajax({
            url: window.listBuildingTenantsUrl,
            type: "POST",
            async: true,
            dataType: "json",
            data: {
                "buildingId": buildingId,
                "days": days
            },
            cache: self.cacheEnable,
            success: function (response) {
                $('#reports-table').find('[name=tenantTemplate]:visible').remove();
                var tenantTemplate = $('#reports-table').find('[name=tenantTemplate]');
                for (var i = 0; i < response.length; i++) {
                    var item = tenantTemplate.clone();
                    item.find('[name="tenant-name"]').replaceWith(response[i].CompanyName);
                    item.find('[name="contact-name"]').replaceWith(response[i].EmailAddress);
                    tenantTemplate.before(item);
                    item.show();
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });
    };

    this.Start = function () {
        self.DisplayBuilding();

        $('#reports_head').find('[name=select-building]').change(function (event) {
            self.DisplayBuilding();

            return false;
        });

        $('#reports_toolbar').find('[name=search-days]').change(function (event) {
            self.DisplayBuilding();

            return false;
        });
    };

    self.Start();
}