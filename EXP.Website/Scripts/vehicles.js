function Vehicles(commonFunctionality, alertsFunctionality) {
    var self = this;
    var makes;
    var models;
    this.common = commonFunctionality;
    this.alerts = alertsFunctionality;
    this.cacheEnable = false;
    this.locCommon = new Common();       

    this.Start = function(){
        self.common.AttachPopup("editVehicleLink", "/ajax/editvehicle.html", self.EditVehicle);
        self.common.AttachPopup("removeVehicleLink", "/ajax/removevehicle.html", self.RemoveVehicle);
        self.common.AttachPopup("newVehicleLink", "/ajax/newvehicle.html", self.NewVehicle);
        self.common.AttachPopup("editVehicleLink1", "/ajax/editvehicle.html", self.EditVehicle);
        self.common.AttachPopup("newVehicleLink1", "/ajax/newvehicle.html", self.NewVehicle);
        
        $.ajax({
            url: window.listMakesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                makes = response;
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });
    };

    this.NewVehicle = function () {
        $("#newVehicleForm").validate({
            rules: {
                vehicle_year: {
                    digits: true,
                    maxlength: 4,
                },
            },
        });

        make = $('[name=vehicle_make]'); 
        make.append("<option value=''>Please select a make</option>"); 

        for (i = 0; i < makes.length; i++) {
            make.append("<option value='" + makes[i].VehicleMakeID + "'>" + makes[i].VehicleMake1 + "</option>");
        }

        $('[name=vehicle_model]').append("<option value=''>Please select a model</option>");
        $('[name=vehicle_year]').append("<option value=''>Please select a year</option>");

        $('[name=vehicle_make]').change( function() {
            makeId = $('[name=vehicle_make]').val();
            model = $('[name=vehicle_model]'); 
            model.empty();
            year = $('[name=vehicle_year]'); 
            year.empty();         
            if (makeId != '') {
                GetModel(makeId); 
            }
            else {
                model.append("<option value=''>Please select a model</option>");
                year.append("<option value=''>Please select a year</option>");
            }
        });

        $('[name=vehicle_model]').change( function() {
            make=$('[name=vehicle_make]');
            var value = $('[name=vehicle_model]').val();
            year = $('[name=vehicle_year]'); 
            year.empty();
            year.append("<option value=''>Please select a year</option>");
            var model_name;
            if (value !='') {
                jQuery.each(models,function() {
                    if(this.VehicleModelID==value)
                    {
                        model_name=this.VehicleModel1;
                    }
                });          
                for (i = 0; i <  models.length; i++) {
                    if (models[i].VehicleModel1==model_name) {
                        year.append("<option value='" + models[i].Year1 + "'>" + models[i].Year1 + "</option>");
                    } 
                } 
            }
            else {
                var rez=[];
                for (i = 0; i <  models.length; i++) {
                    if (models[i].VehicleMakeID==makeId && jQuery.inArray(models[i].Year1,rez)==-1) {
                        rez.push(models[i].Year1);
                        year.append("<option value='" + models[i].Year1 + "'>" + models[i].Year1 + "</option>");
                    } 
                }
           }  
        });

        $('.pop-up').find('[name=save]').click(function (event) {
            event.preventDefault();

            $("#newVehicleForm").validate();
            if (!$("#newVehicleForm").valid()) {
                return false;
            }

            var vehicleModel = new Object();
            vehicleModel.VehicleMakeID = $('.pop-up').find('[name=vehicle_make]').val() != '' ? parseInt($('.pop-up').find('[name=vehicle_make]').val()) : null;
            vehicleModel.VehicleModelID = $('.pop-up').find('[name=vehicle_model]').val() != '' ? parseInt($('.pop-up').find('[name=vehicle_model]').val()) : null;
            vehicleModel.Year = $('.pop-up').find('[name=vehicle_year]').val() != '' ? parseInt($('.pop-up').find('[name=vehicle_year]').val()) : null;
            vehicleModel.LicensePlateNumber = $('.pop-up').find('[name=license_number]').val();
            vehicleModel.Color = $('.pop-up').find('[name=color]').val();
            vehicleModel.PermitNumber = $('.pop-up').find('[name=permit_number]').val();

            $.ajax({
                url: window.newVehicleUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: vehicleModel,

                success: function (newVehicleResponse) {
                    if (newVehicleResponse.success) {
                        self.DisplayVehicle(newVehicleResponse.newVehicleId);
                        $('#overlay').hide();
                        $('.pop-up').remove();
                        $('#spanManageVehicles').show();
                        if (newVehicleResponse.alert != null) {
                            if (newVehicleResponse.deleteAlertId !=0) {
                                self.alerts.RemoveAlert(newVehicleResponse.deleteAlertId);
                            }
                            if (newVehicleResponse.alert.isActive != false) {
                                self.alerts.CreateAlert(newVehicleResponse.alert);
                            }
                        }
                    }
                    else {
                        self.common.ShowErrorMessage(newVehicleResponse.error);
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

    this.EditVehicle = function () {
        $("#editVehicleForm").validate({
            rules: {
                vehicle_year: {
                    digits: true,
                    maxlength: 4,
                },
            },
        });

        make = $('[name=vehicle_make]'); 
        make.append("<option value=''>Please select a make</option>"); 

        for (i = 0; i < makes.length; i++) {
            make.append("<option value='" + makes[i].VehicleMakeID + "'>" + makes[i].VehicleMake1 + "</option>");
        }

        $.ajax({
            url: window.listVehiclesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                var dropDown = $('#vehicle').find('[name=vehicle]');
                dropDown.empty();
                if (response.length == 0) {
                    $('.pop-up').find('[name=save]').css('visibility', 'hidden');
                }
                if (response.length > 0) {
                    $.each(response, function(index, value) {
                        $(dropDown).append('<option value="' + value.VehicleID + '">' + self.GetVehicleName(value) + '</option>');
                    });
                    $('.pop-up').find('[name=vehicle]').change();
                }
                else {
                    $('.pop-up').find('[name=save]').attr("disabled", "disabled");
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });

        $('.pop-up').find('[name=vehicle]').change(function() {
            $.ajax({
                url: window.getVehicleUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: {
                    "vehicleId": $('.pop-up').find('[name=vehicle]').val(),
                },

                success: function (getVehicleResponse) {
                    if (getVehicleResponse.success) {
                        $('.pop-up').find('[name=vehicle_make]').val(getVehicleResponse.vehicle.VehicleMakeID != null ? getVehicleResponse.vehicle.VehicleMakeID : '');
                        GetModels();
                        $('.pop-up').find('[name=vehicle_model]').val(getVehicleResponse.vehicle.VehicleModelID != null ? getVehicleResponse.vehicle.VehicleModelID : '');
                        GetYear();
                        $('.pop-up').find('[name=vehicle_year]').val(getVehicleResponse.vehicle.Year != null ? getVehicleResponse.vehicle.Year : '');
                        $('.pop-up').find('[name=license_number]').val(getVehicleResponse.vehicle.LicensePlateNumber);
                        $('.pop-up').find('[name=color]').val(getVehicleResponse.vehicle.Color);
                        $('.pop-up').find('[name=permit_number]').val(getVehicleResponse.vehicle.PermitNumber);
                    }
                    else {
                        alert(getVehicleResponse.error);
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
        });

        $('[name=vehicle_make]').change(function() {
            GetModels(); 
        });

        function GetModels() {
            makeId = $('[name=vehicle_make]').val();
            model = $('[name=vehicle_model]'); 
            model.empty();
            year = $('[name=vehicle_year]'); 
            year.empty();        
            if (makeId != '') {
                GetModel(makeId);
            }
            else {
                model.append("<option value=''>Please select a model</option>");
            }      
        }

        $('[name=vehicle_model]').change(GetYear);

        function GetYear() {
            make=$('[name=vehicle_make]');
            var value = $('[name=vehicle_model]').val();
            year = $('[name=vehicle_year]'); 
            year.empty();
            year.append("<option value=''>Please select a year</option>");
            var model_name;
            if (value !='') {
                jQuery.each(models,function() {
                    if(this.VehicleModelID==value)
                    {
                        model_name=this.VehicleModel1;
                    }
                });          
                for (i = 0; i <  models.length; i++) {
                    if (models[i].VehicleModel1==model_name) {
                        year.append("<option value='" + models[i].Year1 + "'>" + models[i].Year1 + "</option>");
                    } 
                }   
            }
            else {
                if (models != undefined) {
                    var rez=[];
                    for (i = 0; i <  models.length; i++) {
                        if (models[i].VehicleMakeID==makeId && jQuery.inArray(models[i].Year1,rez)==-1) {
                            rez.push(models[i].Year1);
                            year.append("<option value='" + models[i].Year1 + "'>" + models[i].Year1 + "</option>");
                        } 
                    }
                }
                
           }  
                  
        }

        $('.pop-up').find('[name=save]').click(function (event) {
            event.preventDefault();

            $("#editVehicleForm").validate();
            if (!$("#editVehicleForm").valid()) {
                return false;
            }

            var vehicleModel = new Object();
            vehicleModel.VehicleId = $('.pop-up').find('[name=vehicle]').val();
            vehicleModel.VehicleMakeId = $('.pop-up').find('[name=vehicle_make]').val();
            vehicleModel.VehicleModelId = $('.pop-up').find('[name=vehicle_model]').val();
            vehicleModel.Year = $('.pop-up').find('[name=vehicle_year]').val() != '' ? $('.pop-up').find('[name=vehicle_year]').val() : '';
            vehicleModel.LicensePlateNumber = $('.pop-up').find('[name=license_number]').val();
            vehicleModel.Color = $('.pop-up').find('[name=color]').val();
            vehicleModel.PermitNumber = $('.pop-up').find('[name=permit_number]').val();

            $.ajax({
                url: window.editVehicleUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: vehicleModel,

                success: function (editVehicleResponse) {
                    if (editVehicleResponse.success) {
                        self.ChangeVehicle(editVehicleResponse.VehicleId);
                        $('#overlay').hide();
                        $('.pop-up').remove();
                        if (editVehicleResponse.alert != null) {
                            if (editVehicleResponse.alert.isActive == false) {
                                self.alerts.RemoveAlert(editVehicleResponse.alert.AlertID);
                            }
                            else{
                                self.alerts.CreateAlert(editVehicleResponse.alert);
                            }
                        }
                    }
                    else {
                        self.common.ShowErrorMessage(editVehicleResponse.error);
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

    this.RemoveVehicle = function () {
        var count = 0;
        $.ajax({
            url: window.listVehiclesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                var dropDown = $('#vehicle').find('[name=vehicle]');
                dropDown.empty();
                if (response.length == 0) {
                    $('.pop-up').find('[name=remove]').css('visibility', 'hidden');
                }
                $.each(response, function (index, value) {
                    count++;
                    $(dropDown).append('<option value="' + value.VehicleID + '">' + self.GetVehicleName(value) + '</option>');
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });

        $('.pop-up').find('[name=remove]').click(function (event) {
            event.preventDefault();
            if (count == 1) {
                alert("There must be at least one vehicle listed in the system");
            }
            var vehicleModel = new Object();
            vehicleModel.VehicleId = $('.pop-up').find('[name=vehicle]').val();

            $.ajax({
                url: window.removeVehicleUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: vehicleModel,

                success: function (removeVehicleResponse) {
                    if (removeVehicleResponse.success) {
                        self.RemoveVehicleFromList(vehicleModel.VehicleId);
                        $('#overlay').hide();
                        $('.pop-up').remove();
                        if (removeVehicleResponse.vehicles.length > 1) {
                            self.DisplayVehicle(removeVehicleResponse.vehicles[1].VehicleID);
                        }
                        if (removeVehicleResponse.vehicles.length == 0) {
                            $('#spanManageVehicles').hide();
                        }
                        if (removeVehicleResponse.alerts != null) {
                            $.each(removeVehicleResponse.alerts, function() {
                                self.alerts.RemoveAlert(this.AlertID);
                            });
                        }
                    }
                    else {
                        self.common.ShowErrorMessage(removeVehicleResponse.error);
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

    this.GetVehicleName = function(vehicle) {
        var name;
        if (vehicle.VehicleMake == null && vehicle.VehicleModel == null) {
            name = '#' + vehicle.VehicleID + ' Vehicle';
        }
        else {
            name = '#' + vehicle.VehicleID + '-' +
                (vehicle.VehicleMake != null ? vehicle.VehicleMake.VehicleMake1 : '') + ' ' +
                (vehicle.VehicleModel != null ? vehicle.VehicleModel.VehicleModel1 : '');
        }

        return name;
    };

    function GetModel(makeId) {
            $.ajax({
            url: window.listModelsUrl,
            type: "POST",
            async: false,
            dataType: "json",
            cache: self.cacheEnable,
            data: {
                    "makeId": makeId,
            },
            success: function (response) {
                models=response;
                if (models.length!=0) {
                    InsertModels(makeId);
                }  
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });  
    }

    function InsertModels(makeId) {
           var rez=[];
           var rez2=[];
           model.append("<option value=''>Please select a model</option>");
           year.append("<option value=''>Please select a year</option>"); 
           for (i = 0; i <  models.length; i++) {
               if (models[i].VehicleMakeID==makeId && jQuery.inArray(models[i].VehicleModel1,rez)==-1) {
                   rez.push(models[i].VehicleModel1);
                   model.append("<option value='" + models[i].VehicleModelID + "'>" + models[i].VehicleModel1 + "</option>");
               }
                
               if (models[i].VehicleMakeID==makeId && jQuery.inArray(models[i].Year1,rez2)==-1) {
                   rez2.push(models[i].Year1);
                   year.append("<option value='" + models[i].Year1 + "'>" + models[i].Year1 + "</option>");
               } 
           }  
    }

    this.DisplayVehicle = function (vehicleID) {
        if ($('#vehicles').children().length < 3) {
            $.ajax({
                url: window.getVehicleUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: {
                    "vehicleId": vehicleID,
                },
                success: function (response) {
                    if (response.success) {

                        if ($('#vehicles span').length == 0) {
                            $('#vehicles').children().last().remove();
                        }
                        $('#vehicles').append('<span id="vehicle' + vehicleID + '">' + (response.vehicle.VehicleMake != null ? response.vehicle.VehicleMake.VehicleMake1 : '') + ' ' + (response.vehicle.VehicleModel != null ? response.vehicle.VehicleModel.VehicleModel1 : '') + '</span>');
                        if ($('#vehicles span').length == 1) {
                            $('#vehicles').append('<br/>');
                        }
                    }
                    else {
                        self.common.ShowErrorMessage(response.error);
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
        }
    };

    this.ChangeVehicle = function (vehicleId) {
        $.ajax({
            url: window.getVehicleUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            data: {
                "vehicleId": vehicleId,
            },
            success: function (response) {
                if (response.success) {
                    var replaceRow = $('#vehicle' + response.vehicle.VehicleID).closest("span");
                    replaceRow.text((response.vehicle.VehicleMake != null ? response.vehicle.VehicleMake.VehicleMake1 : '') + " " + (response.vehicle.VehicleModel != null ? response.vehicle.VehicleModel.VehicleModel1 : ''));
                }
                else {
                    self.common.ShowErrorMessage(response.error);
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });
    };

    this.RemoveVehicleFromList = function (vehicleID) {
        if ($('#vehicles').find('#vehicle' + vehicleID).is(":first-child")) {
            $('#vehicles br').remove();
            $('#vehicles').find('#vehicle' + vehicleID).remove();
            $('#vehicles').append('<br/>');
        }
        else {
            $('#vehicles').find('#vehicle' + vehicleID).remove();
        }
    };

    self.Start();
}