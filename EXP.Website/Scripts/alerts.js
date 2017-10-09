function Alerts(commonFunctionality) {
    var self = this;
    this.common = commonFunctionality;
    this.cacheEnable = false;
    var id;
    var url;
    var method;
    this.locCommon = new Common();

    this.Start = function () {
        self.AttachDeleteAlerts();
    };

    this.AttachDeleteAlerts = function () {
        var deleteButton = $('#alerts_table').find('[name=Delete]');
        deleteButton.click(function (event) {
            event.preventDefault();

            var alertIds = [];
            $('#alerts_table').find('input:checkbox:checked').each(function () {
                alertIds.push(parseInt(this.id.replace('alert', ''), 10));
            });

            if (alertIds.length == 0) {
                alert('Please select any alert');
                return false;
            }

            $.ajax({
                url: window.deleteAlertUrl,
                type: "POST",
                contentType: 'application/json',
                cache: self.cacheEnable,
                data: JSON.stringify({ alertIds: alertIds }),

                success: function (deleteAlertResponse) {
                    if (deleteAlertResponse.success) {
                        jQuery.each(alertIds, function () {
                            self.RemoveAlert(this);
                        });
                    }
                    else {                        
                        self.locCommon.ShowErrorBox('', deleteAlertResponse.error);
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {                    
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });

            return false;
        });
    };

    this.RemoveAlert = function (alertId, callback, argument) {
        var closestRow = $('#alert' + alertId).closest("tr");
        closestRow.fadeOut(500, function () {
            closestRow.remove();
            if (callback) {
                callback(argument);
            }
        });
    };

    this.ReplaceAlert = function (alert) {
        var replaceRow = $('#alert' + alert.AlertID).closest("tr");
        replaceRow.find("a").css('id', id).css('href', url).text(alert.AlertText);
    };

    this.CreateAlert = function (alert) {

        switch (alert.EntityTypeID)
        {
            case 1:
                id = "editAddressLink1";
                url = "editaddress.html";
                break;
            case 2:
                id = "editVehicleLink1";
                url = "editvehicle.html";
                break;
            case 3:
                id = "newVehicleLink1";
                url = "newvehicle.html";
                break;
            case 4:
                id = "editParkingLink1";
                url = "";
                break;
            case 5:
                id = "editCreditCardLink1";
                url = "editcreditcard.html";
                break;
            case 6:
                id = "editCreditCardLink1";
                url = "editcreditcard.html";
                break;
            case 7:
                id = "editCreditCardLink1";
                url = "editcreditcard.html";
                break;
        }

        if ($('#alert' + alert.AlertID).length == 0) {
            self.InsertAlertRow(alert);
        } else {
            self.ReplaceAlert(alert);
        }
    };

    this.InsertAlertRow = function (alert) {
        $('#alert').append('<tr style="display: none;"><td class="date_cell"><label class="label_check" for="alert' + alert.AlertID + '">' +
            '<input id="alert' + alert.AlertID + '"  type="checkbox" name="alert' + alert.AlertID + '"/>' +
            $.datepicker.formatDate('mm/dd/yy', new Date()) + '</label></td>' +
            '<td class="info_cell">' +
            '<span><a id=' + id + ' href=' + url + '>' + alert.AlertText + '</a></span></td></tr>');
        $('#alert' + alert.AlertID).closest("tr").fadeIn(500, function () {
            $('#alert' + alert.AlertID).click(function () {
                self.common.setupLabel();
            });
        });
    };

    self.Start();
}