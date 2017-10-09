function PersonalData(commonFunctionality, alertsFunctionality) {
    var self = this;
    this.common = commonFunctionality;
    this.alerts = alertsFunctionality;
    this.cacheEnable = false;
    this.locCommon = new Common();  

    this.Start = function () {
        self.common.AttachPopup("userNameLink", "/ajax/editpassword.html", self.EditPassword);
        self.common.AttachPopup("editPasswordLink", "/ajax/editpassword.html", self.EditPassword);
        self.common.AttachPopup("editEmailLink", "/ajax/editemail.html", self.EditEmail);
        self.common.AttachPopup("editAddressLink", "/ajax/editaddress.html", self.EditPersonalAddress);
     //   self.common.AttachPopup("newAddressLink", "/ajax/newaddress.html", self.SetPersonalAddress);
        self.common.AttachPopup("editAddressLink1", "/ajax/editaddress.html", self.EditPersonalAddress);
        self.common.AttachPopupforEditBuildingAddress("/ajax/editbuilding.html", self.EditBuilding);
        
    };

    this.OverlayShow = function () {
        var hOverlay = $("#overlay").height();
        var hWrapper = $("#wrapper").height();
        var height = (hOverlay > hWrapper ? hOverlay : hWrapper);
        $('#overlay').height(height);
        $('#overlay').show();
    };


    this.EditPassword = function () {
        $("#editPasswordForm").validate({
            rules: {
                current_password: {
                    required: true,
                },
                new_password1: {
                    required: true,
                    minlength: 6,
                },
                new_password2: {
                    required: true,
                    minlength: 6,
                    equalTo: "[name=new_password1]",
                },
            },
            messages: {
                new_password1: { minlength: "Too short" },
                new_password2: { required: "Password doesn't match", minlength: "Password doesn't match", equalTo: "Password doesn't match" }
            }
        });

        $('.pop-up').find('[name=save]').click(function (event) {
            event.preventDefault();

            $("#editPasswordForm").validate();
            if (!$("#editPasswordForm").valid()) {
                return false;
            }

            var editPasswordModel = new Object();
            editPasswordModel.OldPassword = $('.pop-up').find('[name=current_password]').val();
            editPasswordModel.NewPassword = $('.pop-up').find('[name=new_password1]').val();
            $.ajax({
                url: window.changePasswordUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: editPasswordModel,

                success: function (changePasswordResponse) {
                    if (changePasswordResponse.success) {
                        $('#overlay').hide();
                        $('.pop-up').remove();
                    }
                    else {
                        self.common.ShowErrorMessage(changePasswordResponse.error);
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

    this.EditEmail = function () {
        $("#editEmailForm").validate({
            rules: {
                email: {
                    required: true,
                    email: true,
                },
                email2: {
                    required: true,
                    email: true,
                    equalTo: "[name=email]"
                },
            },
        });

        $('.pop-up').find('[name=save]').click(function (event) {
            event.preventDefault();

            $("#editEmailForm").validate();
            if (!$("#editEmailForm").valid()) {
                return false;
            }

            var editEmailModel = new Object();
            editEmailModel.NewEmail = $('.pop-up').find('[name=email]').val();
            $.ajax({
                url: window.changeEmailUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: editEmailModel,

                success: function (changeEmailResponse) {
                    if (changeEmailResponse.success) {
                        $('#overlay').hide();
                        $('.pop-up').remove();
                        $('#emailAddress').html(editEmailModel.NewEmail);
                    }
                    else {
                        self.common.ShowErrorMessage(changeEmailResponse.error);
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

    this.ChangeBuildingAddressInfo = function (editBuildingAddress, forUpdate, address2) {
        var FindBuilding = '.column[data-id=' + editBuildingAddress.BuildingID + ']';
        
        var AppendBuilding = '<div class="column" data-id="' + editBuildingAddress.BuildingID + '" style="height:auto; ">' +
            '</div>';

        if (forUpdate == true) {
            $(FindBuilding).empty();
        } else {
            $('#buildings_body').append(AppendBuilding);
        }           
        var comma = (address2 === "") ? "" : ",";

        var InnerData = '<div class="panel_content" style="height:auto" >' +
                                        '<p>' +
                                            '<span class="bold">' + editBuildingAddress.BuildingName + '</span><br />' +
                                             editBuildingAddress.Address1 + comma + '&nbsp' + '<span data-id="address2" >' +  address2 + '</span><br />' +
                                            editBuildingAddress.City + ',&nbsp' + editBuildingAddress.StateName + ',&nbsp' + editBuildingAddress.ZipCode + '<br />' +
                                            '<a data-id="' + editBuildingAddress.BuildingID + '" href="/editemail.html">Edit</a> | <a data-id="' + editBuildingAddress.BuildingID + '" href="addtenant.html">Add Tenant </a>| <a data-id="' + editBuildingAddress.BuildingID + '" href="#">Remove</a>' +
                                       '</p>' +
                        '</div>';
        //(forUpdate == true) ? editBuildingAddress.Address2 : '&nbsp'
        $(FindBuilding).append(InnerData);

        var buildingId;

        //$(FindBuilding).find('a[href="/editemail.html"]').click(function (event) {
        $(FindBuilding +' ' + 'a[href="/editemail.html"]').click(function (event) {
            event.preventDefault();
            buildingId = $(this).attr('data-id');

            $.ajax({
                url: "/ajax/editbuilding.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    self.OverlayShow();
                    $('.pop-up').remove();
                    $('body').append(html);
                    $("[name='buildingId']").attr("value", buildingId);
                    $('.pop-up').find('.close,[name=cancel]').click(function (event1) {
                        event1.preventDefault();

                        $('#overlay').hide();
                        $('.pop-up').remove();

                        return false;
                    });
                    self.EditBuilding(buildingId);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });

        $(FindBuilding +' ' + 'a[href="#"]').each(function () {
            $(this).click(function (event) {
                event.preventDefault();
                buildingId = $(this).attr('data-id');

                $.ajax({
                    url: window.deleteBuildingAddressUrl,
                    type: "POST",
                    dataType: "json",
                    cache: self.cacheEnable,
                    data: {
                        "BuildingId": buildingId,
                    },
                    success: function (response) {
                        if (response.success) {
                            $('.column[data-id=' + buildingId + ']').remove();
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
            });
        });

    };
   

    this.EditBuilding = function (buildingId) {
        $.ajax({
            url: window.listStatesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            async: false,
            success: function (response) {
                $('#dashboard-setup-building-owner').find('[name=state]').empty();
                jQuery.each(response, function () {
                    $('#dashboard-setup-building-owner').find('[name=state]').append("<option value='" + this.StateID + "'>" + this.StateCode + "</option>");
                });
                $('#dashboard-setup-building-owner').find('[name=state]').bindOptions();
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });
                

        $.ajax({
            url: window.getBuildingAddressUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            data: {
                "BuildingId": buildingId,
            },
            success: function (getAddressResponse) {
                if (getAddressResponse.success) {
                    $('.pop-up').find('[name=building_name]').val(getAddressResponse.building.BuildingName);
                    $('.pop-up').find('[name=city]').val(getAddressResponse.building.BuildingAddress.City);
                    $('.pop-up').find('[name=building_address]').val(getAddressResponse.building.BuildingAddress.Address1);
                    $('.pop-up').find('[name=state]').val(getAddressResponse.building.BuildingAddress.StateID);
                    $('.pop-up').find('[name=zip]').val(getAddressResponse.building.BuildingAddress.ZipCode);
                }
                else {
                    self.common.ShowErrorMessage(getAddressResponse.error);
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });

        $('.pop-up').find('[name=save]').click(function (event) {
            event.preventDefault();

            var BuildingAddressModel = new Object();
            BuildingAddressModel.City = $('.pop-up').find('[name=city]').val();
            BuildingAddressModel.Address1 = $('.pop-up').find('[name=building_address]').val();
            BuildingAddressModel.BuildingID = $('.pop-up').find('[name=buildingId]').val();
            BuildingAddressModel.ZipCode = $('.pop-up').find('[name=zip]').val();
            BuildingAddressModel.StateID = $('.pop-up').find('[name=state]').val();
            BuildingAddressModel.BuildingName = $('.pop-up').find('[name=building_name]').val();
            //BuildingAddressModel.Address2 = $('.pop-up').find('[name=address2]').val();

            $.ajax({
                url: window.editBuildingAddressUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: BuildingAddressModel,

                success: function (editBuildingAddressResponse) {
                    if (editBuildingAddressResponse.success) {
                        var address2 = $("[name='address2']").val();
                        $('#overlay').hide();
                        $('.pop-up').remove();
                                                
                        if (editBuildingAddressResponse.editBuildingAddress.Updated) {
                            self.ChangeBuildingAddressInfo(editBuildingAddressResponse.editBuildingAddress, true, address2);
                        } else {
                            self.ChangeBuildingAddressInfo(editBuildingAddressResponse.editBuildingAddress, false, address2);
                        }
                    }
                    else {
                        self.common.ShowErrorMessage(editBuildingAddressResponse.error);
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
        });

        return false;
    };

    

    this.EditPersonalAddress = function () {

        self.SetListStates();

        $.ajax({
            url: window.getPersonallyAddressUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,  
            success: function (getAddressResponse) {
                if (getAddressResponse.success) {
                    $('.pop-up').find('[name=address1]').val(getAddressResponse.address.Address1);
                    $('.pop-up').find('[name=city]').val(getAddressResponse.address.City);
                    $('.pop-up').find('[name=zipcode]').val(getAddressResponse.address.ZipCode);
                    $('.pop-up').find('[name=address2]').val(getAddressResponse.address.Address2);
                    $('.pop-up').find('[name=state]').val(getAddressResponse.address.StateID);
                }
                else {
                    self.common.ShowErrorMessage(getAddressResponse.error);
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });

        $('.pop-up').find('[name=save]').click(function (event) {
            event.preventDefault();

            var personalAddressModel = new Object();
            personalAddressModel.City = $('.pop-up').find('[name=city]').val();
            personalAddressModel.Address1 = $('.pop-up').find('[name=address1]').val();
            personalAddressModel.Address2 = $('.pop-up').find('[name=address2]').val();
            personalAddressModel.Zipcode = $('.pop-up').find('[name=zipcode]').val();
            personalAddressModel.StateId = $('.pop-up').find('[name=state]').val();

            $.ajax({
                url: window.editPersonalAddressUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: personalAddressModel,

                success: function (editPersonalAddressResponse) {
                    if (editPersonalAddressResponse.success) {
                        $('#overlay').hide();
                        $('.pop-up').remove();
                        self.ChangePersonalInfo();
                        if (editPersonalAddressResponse.alert != null) {
                            if (editPersonalAddressResponse.alert.isActive == false) {
                                self.alerts.RemoveAlert(editPersonalAddressResponse.alert.AlertID);
                            }
                            else{
                                self.alerts.CreateAlert(editPersonalAddressResponse.alert);
                            }
                        }
                    }
                    else {
                        self.common.ShowErrorMessage(editPersonalAddressResponse.error);
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

    this.ChangePersonalInfo = function () { 
        $.ajax({
            url: window.getPersonallyAddressUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            success: function (getAddressResponse) {
                if (getAddressResponse.success) {
                    $('#profile_body').find('#personal_address').html((getAddressResponse.address.Address1 != null ? getAddressResponse.address.Address1 : ' ') + '<br />' + (getAddressResponse.address.City != null ? getAddressResponse.address.City : ' ')  + (getAddressResponse.address.ZipCode != null ?  ', ' + getAddressResponse.address.ZipCode : ' '));
                }
                else {
                    self.common.ShowErrorMessage(getUserProfileResponse.error);
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });
    };

    this.SetListStates = function () {
        $.ajax({
            url: window.listStatesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            async: true,
            success: function (response) {
                jQuery.each(response, function () {
                    $('.pop-up').find('[name=state]').append("<option value='" + this.StateID + "'>" + this.StateName + "</option>");
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });
        return;
    };

    self.Start();
}