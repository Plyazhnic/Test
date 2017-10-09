function RegistrationBuildingOwner(setProfileFunction) {
    var self = this;
    this.cacheEnable = false;
    this.buildingOwner = new Object();
    this.setProfileFunction = setProfileFunction;
    this.locCommon = new Common();

    this.Start = function () {
        self.BuildingInformation(false);
    };

    this.Restore = function (step, data) {
        self.buildingOwner = data;
        if (self.buildingOwner.ParkingManagementContact != null) {
            self.buildingOwner.ParkingManagementContact.Same = (self.buildingOwner.Same == null ? false : self.buildingOwner.Same);
        }     
        switch (step) {
            case 13:
                self.BuildingInformation(false);
                break;
            case 14:
                self.OwnershipInformation(false);
                break;
            case 15:
                self.PropertyManagementInformation(false);
                break;
            case 16:
                self.ParkingManagementInformation(false);
                break;
            case 17:
                self.BankAccountSetupInformation(false);
                break;
            case 18:
                self.ParkingLotInformation(false);
                break;
            case 19:
                self.TenantInformation();
                break;
            case 20:
                self.VerifyOwner();
                break;
            case 21:
                self.TenantInformation();
                break;
        }
    };

    this.BuildingInformation = function (isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-building-owner.html", null);

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

        if (self.buildingOwner.Building != null) {
            $('#dashboard-setup-building-owner').find('[name="building_name"]')
                .val(self.buildingOwner.Building.BuildingName);
            $('#dashboard-setup-building-owner').find('[name="building_address"]')
                .val(self.buildingOwner.Building.BuildingAddress.Address1);
            $('#dashboard-setup-building-owner').find('[name="city"]')
                .val(self.buildingOwner.Building.BuildingAddress.City);
            $('#dashboard-setup-building-owner').find('[name="state"]')
                .val(self.buildingOwner.Building.BuildingAddress.StateID).bindOptions();
            $('#dashboard-setup-building-owner').find('[name="zip"]')
                .val(self.buildingOwner.Building.BuildingAddress.ZipCode);
        }

        $('#dashboard-setup-building-owner').find('[name=back]').click(function (event) {
            event.preventDefault();
            $.ajax({
                url: "ajax/dashboard-setup-profile.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.setProfileFunction();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });

        $("#dashboard-setup-building-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function (label) {
                label.not('[for="agree"]').addClass("success");
            },

            rules: {
                building_name: {
                    required: true
                },
                building_address: {
                    required: true
                },
                city: {
                    required: true
                },
                state: {
                    required: true
                },
                zip: {
                    required: true
                },
            },
            messages: {
                building_name: { required: "Enter building name" },
                building_address: { required: "Enter building address" },
                city: { required: "Enter city" },
                state: { required: "Select state" },
                zip: { required: "Enter zip" }
            }
        });

        $('#dashboard-setup-building-owner').find('[name=continue]').click(function (event) {
            event.preventDefault();
            if ($("#dashboard-setup-building-owner-form").valid()) {

                var model = new Object();
                if (building == null) {
                    var building = new Object();
                }
                var BuildingAddress = new Object();
                model.BuildingName = building.BuildingName = $('#dashboard-setup-building-owner').find('[name="building_name"]').val();
                model.Address1 = BuildingAddress.Address1 = $('#dashboard-setup-building-owner').find('[name="building_address"]').val();
                model.City = BuildingAddress.City = $('#dashboard-setup-building-owner').find('[name="city"]').val();
                model.StateID = BuildingAddress.StateID = $('#dashboard-setup-building-owner').find('[name="state"]').val();
                model.State = BuildingAddress.State = $('#dashboard-setup-building-owner').find('[name="state"] option:selected').text();
                model.ZipCode = BuildingAddress.ZipCode = $('#dashboard-setup-building-owner').find('[name="zip"]').val();
                building.BuildingAddress = BuildingAddress;

                self.SendInformationToServer(window.sendBuildingInformationUrl, model, function () {
                    if (self.buildingOwner.Building != null && self.buildingOwner.Building.BankAccount != null) {
                        var BankAccount = self.buildingOwner.Building.BankAccount;
                        self.buildingOwner.Building = building;
                        self.buildingOwner.Building.BankAccount = BankAccount;
                    }
                    else {
                        self.buildingOwner.Building = building;
                    }
                    if (isVerify) {
                        self.VerifyOwner();
                    } else {
                        self.OwnershipInformation(false);
                    }
                    
                });
            }

            return false;
        });
    };

    this.OwnershipInformation = function (isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-owner-owner.html", null);

        $.ajax({
            url: window.ListPhoneTypesUrl,
            type: "POST",
            dataType: "json",
            async: false,
            cache: self.cacheEnable,
            success: function (Response) {
                jQuery.each(Response, function () {
                    $('#dashboard-setup-owner-owner').find('[name=owner_device]').append("<option value='" + this.PhoneTypeID + "'>" + this.PhoneType1 + "</option>");
                });
                $('#dashboard-setup-owner-owner').find('[name=owner_device]').bindOptions();
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });

        if (self.buildingOwner.OwnerContact != null) {
            $('#dashboard-setup-owner-owner').find('[name="owner_building_name"]')
                .val(self.buildingOwner.OwnerContact.Name);
            $('#dashboard-setup-owner-owner').find('[name="owner_first_name"]')
                .val(self.buildingOwner.OwnerContact.FirstName);
            $('#dashboard-setup-owner-owner').find('[name="owner_last_name"]')
                .val(self.buildingOwner.OwnerContact.LastName);
            $('#dashboard-setup-owner-owner').find('[name="owner_email"]')
                .val(self.buildingOwner.OwnerContact.Email);
            $('#dashboard-setup-owner-owner').find('[name="owner_device"]')
                .val(self.buildingOwner.OwnerContact.PhoneTypeID).bindOptions();
            $('#dashboard-setup-owner-owner').find('[name="owner_phone"]')
                .val(self.buildingOwner.OwnerContact.PhoneNumber);
        }
        
        var ownerName = $('#dashboard-setup-owner-owner').find('[name="owner_building_name"]');
        if (ownerName.val() == '') {
            ownerName.val('Example: Sherman Plaza, LCC');
            ownerName.css("color", "#999999");
        }

        ownerName.focus(function () {
            if (this.value === 'Example: Sherman Plaza, LCC') {
                this.value = '';
                ownerName.css("color", "#000000");
            }
        });
        ownerName.blur(function () {
            if (this.value === '') {
                this.value = 'Example: Sherman Plaza, LCC';
                ownerName.css("color", "#999999");
            }
        });

        $('#dashboard-setup-owner-owner').find('[name=back]').click(function (event) {
            event.preventDefault();
            self.BuildingInformation(false);
            return false;
        });

        $.validator.addMethod("phoneformat",
                             function (value, element) {
                                 return value.match(/\(\d\d\d\)\d\d\d\-\d\d\d\d$/);
                             },
                            "Should be the format (000)000-0000"
                            );

        $("input#phone").mask("(999)999-9999");

        $("#dashboard-setup-owner-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function (label) {
                label.not('[for="agree"]').addClass("success");
            },

            rules: {
                owner_building_name: {
                    required: true,
                    notEqual: "Example: Sherman Plaza, LCC"
                },
                owner_first_name: {
                    required: true
                },
                owner_last_name: {
                    required: true
                },
                owner_email: {
                    required: true,
                    email: true
                },
                owner_device: {
                    required: true
                },
                owner_phone: {
                    required: true,
                    phoneformat: true
                },
            },
            messages: {
                owner_building_name: { required: "Enter Building Owner Name/Entity Name", notEqual: "Enter Building Owner Name/Entity Name" },
                owner_first_name: { required: "Enter first name" },
                owner_last_name: { required: "Enter last name" },
                owner_email: { required: "Enter email", email: "Incorrect email format" },
                owner_device: { required: "Select device" },
                owner_phone: { required: "Enter telephone number" }
            }
        });

        $('#dashboard-setup-owner-owner').find('[name=continue]').click(function (event) {
            event.preventDefault();
            
            if ($('#dashboard-setup-owner-owner-form').valid()) {
                var model = new Object();;
                model.Name = $('#dashboard-setup-owner-owner').find('[name="owner_building_name"]').val();
                model.FirstName = $('#dashboard-setup-owner-owner').find('[name="owner_first_name"]').val();
                model.LastName = $('#dashboard-setup-owner-owner').find('[name="owner_last_name"]').val();
                model.Email = $('#dashboard-setup-owner-owner').find('[name="owner_email"]').val();
                model.PhoneTypeID = $('#dashboard-setup-owner-owner').find('[name="owner_device"]').val();
                model.PhoneType = $('#dashboard-setup-owner-owner').find('[name="owner_device"] option:selected').text();
                model.PhoneNumber = $('#dashboard-setup-owner-owner').find('[name="owner_phone"]').val();

                self.SendInformationToServer(window.sendOwnershipInformationUrl, model, function () {
                    self.buildingOwner.OwnerContact = model;
                    if (isVerify) {
                        self.VerifyOwner();
                    } else {
                        self.PropertyManagementInformation(false);
                    }
                });
            }
            
            return false;
        });
    };

    this.PropertyManagementInformation = function (isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-management-owner.html", null);

        $.ajax({
            url: window.ListPhoneTypesUrl,
            type: "POST",
            dataType: "json",
            async: false,
            cache: self.cacheEnable,
            success: function (Response) {
                jQuery.each(Response, function () {
                    $('#dashboard-setup-management-owner').find('[name=contact_device]').append("<option value='" + this.PhoneTypeID + "'>" + this.PhoneType1 + "</option>");
                });
                $('#dashboard-setup-management-owner').find('[name=contact_device]').bindOptions();
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });

        if (self.buildingOwner.PropertyManagementContact != null) {
            var o = self.buildingOwner.PropertyManagementContact;
            self.RestoreState('#dashboard-setup-management-owner', 'management_name', o.Name);
            self.RestoreState('#dashboard-setup-management-owner', 'contact_first_name', o.FirstName);
            self.RestoreState('#dashboard-setup-management-owner', 'contact_last_name', o.LastName);
            self.RestoreState('#dashboard-setup-management-owner', 'contact_email', o.Email);
            self.RestoreState('#dashboard-setup-management-owner', 'contact_device', o.PhoneTypeID).bindOptions();
            self.RestoreState('#dashboard-setup-management-owner', 'contact_phone', o.PhoneNumber);
        }

        $('#dashboard-setup-management-owner').find('[name=back]').click(function (event) {
            event.preventDefault();
            self.OwnershipInformation(false);
            return false;
        });

        $.validator.addMethod("phoneformat",
                              function (value, element) {
                                  return value.match(/\(\d\d\d\)\d\d\d\-\d\d\d\d$/);
                              },
                             "Should be the format (000)000-0000"
                             );

        $("input#phone").mask("(999)999-9999");

        $("#dashboard-setup-management-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function (label) {
                label.not('[for="agree"]').addClass("success");
            },

            rules: {
                management_name: {
                    required: true
                },
                contact_first_name: {
                    required: true
                },
                contact_last_name: {
                    required: true
                },
                contact_email: {
                    required: true,
                    email: true
                },
                contact_device: {
                    required: true
                },
                contact_phone: {
                    required: true,
                    phoneformat: true
                },
            },
            messages: {
                management_name: { required: "Enter Company Name" },
                contact_first_name: { required: "Enter first name" },
                contact_last_name: { required: "Enter last name" },
                contact_email: { required: "Enter email", email: "Incorrect email format" },
                contact_device: { required: "Select device" },
                contact_phone: { required: "Enter telephone number" }
            }
        });

        $('#dashboard-setup-management-owner').find('[name=continue]').click(function (event) {
            event.preventDefault();

            if ($('#dashboard-setup-management-owner-form').valid()) {
                var model = new Object();;
                model.Name = $('#dashboard-setup-management-owner').find('[name="management_name"]').val();
                model.FirstName = $('#dashboard-setup-management-owner').find('[name="contact_first_name"]').val();
                model.LastName = $('#dashboard-setup-management-owner').find('[name="contact_last_name"]').val();
                model.Email = $('#dashboard-setup-management-owner').find('[name="contact_email"]').val();
                model.PhoneTypeID = $('#dashboard-setup-management-owner').find('[name="contact_device"]').val();
                model.PhoneType = $('#dashboard-setup-management-owner').find('[name="contact_device"] option:selected').text();
                model.PhoneNumber = $('#dashboard-setup-management-owner').find('[name="contact_phone"]').val();

                self.SendInformationToServer(window.sendPropertyManagementInformationUrl, model, function () {
                    self.buildingOwner.PropertyManagementContact = model;
                    if (isVerify) {
                        self.VerifyOwner();
                    } else {
                        self.ParkingManagementInformation(false);
                    }
                });
            }

            return false;
        });
    };

    this.ParkingManagementInformation = function (isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-management2-owner.html", null);

        $("#dashboard-setup-management2-owner").find("[name='contactColumn']").hide();

        $.ajax({
            url: window.ListPhoneTypesUrl,
            type: "POST",
            dataType: "json",
            async: false,
            cache: self.cacheEnable,
            success: function (response) {
                jQuery.each(response, function () {
                    $('#dashboard-setup-management2-owner').find('[name=contact_device]').append("<option value='" + this.PhoneTypeID + "'>" + this.PhoneType1 + "</option>");
                });
                $('#dashboard-setup-management2-owner').find('[name=contact_device]').bindOptions();
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });
        
        if (self.buildingOwner.ParkingManagementContact != null) {
            $("#dashboard-setup-management2-owner").find("[name='contactColumn']").show();
            var o = self.buildingOwner.ParkingManagementContact;
            $('#dashboard-setup-management2-owner').find(o.Same ? 'label[for="incharge_yes"]' : 'label[for="incharge_no"]').addClass('r_on');//.find('label[for="' + o.Same ? 'incharge_yes' : 'incharge_no' + '"]').addClass('r_on');
            self.RestoreState('#dashboard-setup-management2-owner', 'management_name', o.Name);
            self.RestoreState('#dashboard-setup-management2-owner', 'contact_first_name', o.FirstName);
            self.RestoreState('#dashboard-setup-management2-owner', 'contact_last_name', o.LastName);
            self.RestoreState('#dashboard-setup-management2-owner', 'contact_email', o.Email);
            self.RestoreState('#dashboard-setup-management2-owner', 'contact_device', o.PhoneTypeID);
            self.RestoreState('#dashboard-setup-management2-owner', 'contact_phone', o.PhoneNumber);
        }

        $('#dashboard-setup-management2-owner').find('#incharge_yes').change(function () {
            if ($('.label_radio input:checked').val() == 'yes') {
                $('#company_type').text("Property");
                $("#dashboard-setup-management2-owner").find("[name='contactColumn']").show();
                var o1 = self.buildingOwner.PropertyManagementContact;
                self.RestoreState('#dashboard-setup-management2-owner', 'management_name', o1.Name);
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_first_name', o1.FirstName);
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_last_name', o1.LastName);
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_email', o1.Email);
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_device', o1.PhoneTypeID);
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_phone', o1.PhoneNumber);
            }
        });

        $('#dashboard-setup-management2-owner').find('#incharge_no').change(function () {
            if ($('.label_radio input:checked').val() == 'no') {
                $('#company_type').text("Parking");
                $("#dashboard-setup-management2-owner").find("[name='contactColumn']").show();
                self.RestoreState('#dashboard-setup-management2-owner', 'management_name', "");
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_first_name', "");
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_last_name', "");
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_email', "");
                self.RestoreState('#dashboard-setup-management2-owner', 'contact_phone', "");
            }
        });
        
        $('#dashboard-setup-management2-owner').find('[name=back]').click(function (event) {
            event.preventDefault();
            self.PropertyManagementInformation(false);
            return false;
        });

        $.validator.addMethod("phoneformat",
                              function (value, element) {
                                  return value.match(/\(\d\d\d\)\d\d\d\-\d\d\d\d$/);
                              },
                             "Should be the format (000)000-0000"
                             );

        $("input#phone").mask("(999)999-9999");

        $("#dashboard-setup-management2-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function (label) {
                label.not('[for="agree"]').addClass("success");
            },

            rules: {
                management_name: {
                    required: true
                },
                contact_first_name: {
                    required: true
                },
                contact_last_name: {
                    required: true
                },
                contact_email: {
                    required: true,
                    email: true
                },
                contact_device: {
                    required: true
                },
                contact_phone: {
                    required: true,
                    phoneformat: true
                },
            },
            messages: {
                management_name: { required: "Enter Company Name" },
                contact_first_name: { required: "Enter first name" },
                contact_last_name: { required: "Enter last name" },
                contact_email: { required: "Enter email", email: "Incorrect email format" },
                contact_device: { required: "Select device" },
                contact_phone: { required: "Enter telephone number" }
            }
        });

        $('#dashboard-setup-management2-owner').find('[name=continue]').click(function (event) {
            event.preventDefault();

            if ($('#dashboard-setup-management2-owner-form').valid()) {
                var model = new Object();
                model.Same = $('#dashboard-setup-management2-owner').find('label[for="incharge_yes"]').hasClass('r_on');
                model.Name = $('#dashboard-setup-management2-owner').find('[name="management_name"]').val();
                model.FirstName = $('#dashboard-setup-management2-owner').find('[name="contact_first_name"]').val();
                model.LastName = $('#dashboard-setup-management2-owner').find('[name="contact_last_name"]').val();
                model.Email = $('#dashboard-setup-management2-owner').find('[name="contact_email"]').val();
                model.PhoneTypeID = $('#dashboard-setup-management2-owner').find('[name="contact_device"]').val();
                model.PhoneType = $('#dashboard-setup-management2-owner').find('[name="contact_device"] option:selected').text();
                model.PhoneNumber = $('#dashboard-setup-management2-owner').find('[name="contact_phone"]').val();

                self.SendInformationToServer(window.sendParkingManagementInformationUrl, model, function () {
                    self.buildingOwner.ParkingManagementContact = model;
                    self.buildingOwner.Same = model.Same;
                    if (isVerify) {
                        self.VerifyOwner();
                    } else {
                        self.BankAccountSetupInformation(false);
                    }
                });
            }

            return false;
        });
    };

    this.BankAccountSetupInformation = function (isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-banking-owner.html", null);

        $('input[name=routing_number]').mask("999999999");

        if (self.buildingOwner.Building.BankAccount != null) {
            var o = self.buildingOwner.Building.BankAccount;
            self.RestoreState('#dashboard-setup-banking-owner', 'bank_name', o.BankName);
            self.RestoreState('#dashboard-setup-banking-owner', 'account_number', o.CheckingAccountNumber);
            self.RestoreState('#dashboard-setup-banking-owner', 'account_holder_name', o.NameOnAccount);
            self.RestoreState('#dashboard-setup-banking-owner', 'routing_number', o.RoutingNumber);
        }

        $('#dashboard-setup-banking-owner').find('[name=back]').click(function (event) {
            event.preventDefault();
            self.ParkingManagementInformation(false);
            return false;
        });

        $("#dashboard-setup-banking-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function (label) {
                label.not('[for="agree"]').addClass("success");
            },

            rules: {
                bank_name: {
                    required: true
                },
                account_number: {
                    required: true
                },
                account_holder_name: {
                    required: true
                },
                routing_number: {
                    required: true,
                    routignumber: true,
                },
            },
            messages: {
                bank_name: { required: "Enter Bank Name" },
                account_number: { required: "Enter Account Number" },
                account_holder_name: { required: "Enter Account Holder Name" },
                routing_number: { required: "Enter Bank Routing Number" },
            }
        });

        $('#dashboard-setup-banking-owner').find('[name=continue]').click(function (event) {
            event.preventDefault();

            if ($('#dashboard-setup-banking-owner-form').valid()) {
                var model = new Object();
                var onlineCheck = new Object();
                model.BankName = onlineCheck.BankName = $('#dashboard-setup-banking-owner').find('[name="bank_name"]').val();
                model.AccountNumber = onlineCheck.CheckingAccountNumber = $('#dashboard-setup-banking-owner').find('[name="account_number"]').val();
                model.AccountHolderName = onlineCheck.NameOnAccount = $('#dashboard-setup-banking-owner').find('[name="account_holder_name"]').val();
                model.BankRoutingNumber = onlineCheck.RoutingNumber = $('#dashboard-setup-banking-owner').find('[name="routing_number"]').val();

                self.SendInformationToServer(window.sendBankAccountInformationUrl, model, function () {
                    self.buildingOwner.Building.BankAccount = onlineCheck;
                    if (isVerify) {
                        self.VerifyOwner();
                    } else {
                        self.ParkingLotInformation(false);
                    }
                });
            }

            return false;
        });
    };

    this.ParkingLotInformation = function(isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-parking1-owner.html", null);

        if (self.buildingOwner.HandlerType != null && self.buildingOwner.HandlerType > 0) {
            switch (self.buildingOwner.HandlerType) {
                case 1:
                    $('#dashboard-setup-parking1-owner').find('label[for=property_manager]').addClass('r_on');
                    break;
                case 2:
                    $('#dashboard-setup-parking1-owner').find('label[for=parking_manager]').addClass('r_on');
                    break;
                case 3:
                    $('#dashboard-setup-parking1-owner').find('label[for=owner_manager]').addClass('r_on');
                    break;
            }
        }

        $('#dashboard-setup-parking1-owner').find('[name=back]').click(function(event) {
            event.preventDefault();
            self.BankAccountSetupInformation(false);
            return false;
        });

        $('#dashboard-setup-parking1-owner').find('[name=continue]').click(function(event) {
            event.preventDefault();
            if (self.buildingOwner.ParkingLot == null) {
                self.buildingOwner.ParkingLot = new Object();
            }
            if($('#dashboard-setup-parking1-owner').find('label[for="property_manager"]').hasClass("r_on")) {
                self.buildingOwner.HandlerType = 1;
            } else if ($('#dashboard-setup-parking1-owner').find('label[for="parking_manager"]').hasClass("r_on")) {
                self.buildingOwner.HandlerType = 2;
            } else if ($('#dashboard-setup-parking1-owner').find('label[for="owner_manager"]').hasClass("r_on")) {
                self.buildingOwner.HandlerType = 3;
            } else {
                alert("Please select who will be handling the buildings parking responsiblities");
                return false;
            }
            var o = new Object();
            o.handlerType = self.buildingOwner.HandlerType;

            if (self.buildingOwner.HandlerType == 1) {
                self.PropertyManagementNotifications(function () {
                    self.SendInformationToServer(window.sendHandlerInformationUrl, o, function () {
                        self.ParkingLotInformationReserved(isVerify);
                    });
                });
            }
            else {
                self.SendInformationToServer(window.sendHandlerInformationUrl, o, function () {
                    self.ParkingLotInformationReserved(isVerify);
                });
            }
            return false;
        });
    };

    this.ParkingLotInformationReserved = function(isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-parking2-owner.html", null);

        $('#dashboard-setup-parking2-owner').find('[name="reserved_parking_rate"]').maskMoney({ symbol: '$ ', symbolStay: true });

        if (self.buildingOwner.StallReserved != null) {
            var o = self.buildingOwner.StallReserved;
            self.RestoreState('#dashboard-setup-parking2-owner', 'number_reserved_stalls', o.NumberStalls);
            self.RestoreState('#dashboard-setup-parking2-owner', 'reserved_stall_location', o.StallLocation);
            self.RestoreState('#dashboard-setup-parking2-owner', 'reserved_parking_rate', '$ ' + o.Rate);
            self.RestoreState('#dashboard-setup-parking2-owner', 'reserved_stall_numbers', o.StallNumber);
        }

        $('#dashboard-setup-parking2-owner').find('[name=back]').click(function(event) {
            event.preventDefault();
            self.ParkingLotInformation(false);
            return false;
        });

        $("#dashboard-setup-parking2-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function (label) {
                label.not('[for="agree"]').addClass("success");
            },

            rules: {
                number_reserved_stalls: {
                    required: true,
                    digits: true,
                },
                reserved_stall_location: {
                    required: true
                },
                reserved_parking_rate: {
                    required: true,
                    maxlength: 10,
                },
                reserved_stall_numbers: {
                    digits: true,
                    required: true,
                    range: [1, 50],
                },
            },
            messages: {
                number_reserved_stalls: { required: "Enter Number of Reserved Parking Stalls", digits: "Wrong number" },
                reserved_stall_location: { required: "Enter Reserved Stall Location" },
                reserved_parking_rate: { required: "Enter Reserved Parking Rate", maxlength: "Maximum 6 digits" },
                reserved_stall_numbers: { required: "Enter Reserved Stall Number", digits: "Wrong number", range: "Value should be between 1 and 50" },
            }
        });

        $('#dashboard-setup-parking2-owner').find('[name=continue]').click(function(event) {
            event.preventDefault();

            if ($('#dashboard-setup-parking2-owner-form').valid()) {
                var model = new Object();;
                model.NumberStalls = $('#dashboard-setup-parking2-owner').find('[name="number_reserved_stalls"]').val();
                model.StallLocation = $('#dashboard-setup-parking2-owner').find('[name="reserved_stall_location"]').val();
                model.Rate = $('#dashboard-setup-parking2-owner').find('[name="reserved_parking_rate"]').val().replace('$ ','');
                model.StallNumber = $('#dashboard-setup-parking2-owner').find('[name="reserved_stall_numbers"]').val();
                
                self.SendInformationToServer(window.sendParkingLotInformationReservedUrl, model, function () {
                    self.buildingOwner.StallReserved = model;
                    self.ParkingLotInformationUnreserved(isVerify);
                });
            }

            return false;
        });
    };

    this.ParkingLotInformationUnreserved = function(isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-parking3-owner.html", null);

        $('#dashboard-setup-parking3-owner').find('[name="unreserved_parking_rate"]').maskMoney({ symbol: '$ ', symbolStay: true });
        $('#dashboard-setup-parking3-owner').find('[name="unreserved_parking_oversell"]').maskMoney({ symbol: '$ ', symbolStay: true });

        $('#dashboard-setup-parking3-owner').find('[name=oversell]').change(function () {
            if ($('.label_radio input:checked').val() == 'yes') {
                $("#dashboard-setup-parking3-owner").find("[name=oversellPanel]").show();
                $('#dashboard-setup-parking3-owner').find("[name=unreserved_parking_oversell]").rules("add", { required: true, maxlength: 9, messages: { required: "Enter Unreserved Oversell number", maxlength: "Maximum 5 digits" } });
            }
            else {
                $("#dashboard-setup-parking3-owner").find("[name=oversellPanel]").hide();
                $("#dashboard-setup-parking3-owner").find("[name=unreserved_parking_oversell]").rules("remove");
            }
        });

        if (self.buildingOwner.StallUnreserved != null) {
            var o = self.buildingOwner.StallUnreserved;
            self.RestoreState('#dashboard-setup-parking3-owner', 'number_unreserved_stalls', o.NumberStalls);
            self.RestoreState('#dashboard-setup-parking3-owner', 'unreserved_stall_location', o.StallLocation);
            self.RestoreState('#dashboard-setup-parking3-owner', 'unreserved_parking_rate', '$ ' + o.Rate);            
            if (o.OverSell != 0) {
                self.RestoreState('#dashboard-setup-parking3-owner', 'unreserved_parking_oversell', '$ ' + o.OverSell);
                $("#dashboard-setup-parking3-owner").find("[for=oversell_yes]").addClass('r_on');
                $("#dashboard-setup-parking3-owner").find("[name=oversellPanel]").show();
            }
            else {
                $("#dashboard-setup-parking3-owner").find("[for=oversell_no]").addClass('r_on');
            }
        }
        else {
            $("#dashboard-setup-parking3-owner").find("[for=oversell_no]").addClass('r_on');
        }

        $('#dashboard-setup-parking3-owner').find('[name=back]').click(function(event) {
            event.preventDefault();
            self.ParkingLotInformationReserved(isVerify);
            return false;
        });

        $("#dashboard-setup-parking3-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function (label) {
                label.not('[for="agree"]').addClass("success");
            },

            rules: {
                number_unreserved_stalls: {
                    digits: true,
                },
                unreserved_stall_location: {
                    required: true
                },
                unreserved_parking_rate: {
                    required: true,
                    maxlength: 10,
                },
                unreserved_parking_oversell: {
                    maxlength: 9,
                },
            },
            messages: {
                number_unreserved_stalls: { digits: "Wrong number" },
                unreserved_stall_location: { required: "Enter Unreserved Stall Location" },
                unreserved_parking_rate: { required: "Enter Unreserved Parking Rate", maxlength: "Maximum 6 digits" },
                unreserved_parking_oversell: { required: "Enter Unreserved Parking Oversell", maxlength: "Maximum 5 digits" },
            }
        });

        $('#dashboard-setup-parking3-owner').find('[name=continue]').click(function(event) {
            event.preventDefault();

            if ($('#dashboard-setup-parking3-owner-form').valid()) {
                var model = new Object();
                model.NumberStalls = ($('#dashboard-setup-parking3-owner').find('[name="number_unreserved_stalls"]').val() == '' ? 0 : $('#dashboard-setup-parking3-owner').find('[name="number_unreserved_stalls"]').val());
                model.StallLocation = $('#dashboard-setup-parking3-owner').find('[name="unreserved_stall_location"]').val();
                model.Rate = $('#dashboard-setup-parking3-owner').find('[name="unreserved_parking_rate"]').val().replace('$ ', '');
                if ($('.label_radio input:checked').val() == 'yes') {
                    $('#dashboard-setup-parking3-owner').find('[name="unreserved_parking_oversell"]')
                    model.OverSell = $('#dashboard-setup-parking3-owner').find('[name="unreserved_parking_oversell"]').val().replace('$ ', '');
                }
                else {
                    model.OverSell = 0;
                }

                self.SendInformationToServer(window.sendParkingLotInformationUnreservedUrl, model, function () {
                    self.buildingOwner.StallUnreserved = model;
                    self.ParkingLotInformationVisitor(isVerify);
                });
            }

            return false;
        });
    };

    this.ParkingLotInformationVisitor = function(isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-parking5-owner.html", null);

        $('#dashboard-setup-parking5-owner').find('[name="visitor_parking_rate"]').maskMoney({ symbol: '$ ', symbolStay: true });

        if (self.buildingOwner.StallVisitors != null) {
            var o = self.buildingOwner.StallVisitors;
            self.RestoreState('#dashboard-setup-parking5-owner', 'number_visitor_stalls', o.NumberStalls);
            self.RestoreState('#dashboard-setup-parking5-owner', 'visitor_stall_location', o.StallLocation);
            self.RestoreState('#dashboard-setup-parking5-owner', 'visitor_parking_rate', '$ ' + o.Rate);
        }

        $('#dashboard-setup-parking5-owner').find('[name=back]').click(function(event) {
            event.preventDefault();
            self.ParkingLotInformationUnreserved(isVerify);
            return false;
        });

        $("#dashboard-setup-parking5-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function (label) {
                label.not('[for="agree"]').addClass("success");
            },

            rules: {
                number_visitor_stalls: {
                    required: true,
                    digits: true,
                },
                visitor_stall_location: {
                    required: true
                },
                visitor_parking_rate: {
                    required: true,
                    maxlength: 10,
                },
            },
            messages: {
                number_visitor_stalls: { required: "Enter Number of Visitor Parking Stalls", digits: "Wrong number" },
                visitor_stall_location: { required: "Enter Visitor Stall Location" },
                visitor_parking_rate: { required: "Enter Visitor Parking Rate", maxlength: "Maximum 6 digits" },
            }
        });

        $('#dashboard-setup-parking5-owner').find('[name=continue]').click(function(event) {
            event.preventDefault();

            if ($('#dashboard-setup-parking5-owner-form').valid()) {
                var model = new Object();
                model.NumberStalls = $('#dashboard-setup-parking5-owner').find('[name="number_visitor_stalls"]').val();
                model.StallLocation = $('#dashboard-setup-parking5-owner').find('[name="visitor_stall_location"]').val();
                model.Rate = $('#dashboard-setup-parking5-owner').find('[name="visitor_parking_rate"]').val().replace('$ ', '');

                self.SendInformationToServer(window.sendParkingLotInformationVisitorUrl, model, function () {
                    self.buildingOwner.StallVisitors = model;
                    self.ParkingLotInformationOverview(isVerify);
                });
            }

            return false;
        });
    };
    
    this.ParkingLotInformationOverview = function (isVerify) {
        self.LoadTemplate("ajax/dashboard-setup-parking6-owner.html", null);

        self.RestoreState('#dashboard-setup-parking6-owner', 'total_reserved_stalls', self.buildingOwner.StallReserved.NumberStalls);
        self.RestoreState('#dashboard-setup-parking6-owner', 'total_reserved_stalls_cost', '$ ' + self.buildingOwner.StallReserved.Rate);
        self.RestoreState('#dashboard-setup-parking6-owner', 'total_visitor_stalls', self.buildingOwner.StallVisitors.NumberStalls);
        self.RestoreState('#dashboard-setup-parking6-owner', 'total_visitor_stalls_cost', '$ ' + self.buildingOwner.StallVisitors.Rate);
        self.RestoreState('#dashboard-setup-parking6-owner', 'total_unreserved_stalls', self.buildingOwner.StallUnreserved.NumberStalls);
        self.RestoreState('#dashboard-setup-parking6-owner', 'total_unreserved_stalls_cost', '$ ' + self.buildingOwner.StallUnreserved.Rate);

        $('#dashboard-setup-parking6-owner').find('[name=reservedEdit]').click(function (event) {
            event.preventDefault();
            self.ParkingLotInformationReserved(isVerify);
            return false;
        });

        $('#dashboard-setup-parking6-owner').find('[name=unreservedEdit]').click(function (event) {
            event.preventDefault();
            self.ParkingLotInformationUnreserved(isVerify);
            return false;
        });

        $('#dashboard-setup-parking6-owner').find('[name=visitorEdit]').click(function (event) {
            event.preventDefault();
            self.ParkingLotInformationVisitor(isVerify);
            return false;
        });

        $('#dashboard-setup-parking6-owner').find('[name=back]').click(function (event) {
            event.preventDefault();
            self.ParkingLotInformationVisitor(isVerify);
            return false;
        });

        $('#dashboard-setup-parking6-owner').find('[name=continue]').click(function (event) {
            event.preventDefault();
            if (isVerify) {
                self.VerifyOwner();
            } else {
                self.TenantInformation();
            }
            return false;
        });
    };

    this.TenantInformation = function () {
        self.LoadTemplate("ajax/dashboard-setup-tenant-owner.html", null);
        var count = 1;

        $("#dashboard-setup-tenant-owner-form").validate({
            validClass: "success",
            errorClass: "error",
            success: function(label) {
                label.not('[for="agree"]').addClass("success");
            },
        });

        $('#dashboard-setup-tenant-owner').find('[name=addTenant]').click(function (event) {
            event.preventDefault();

            var clone = $('#dashboard-setup-tenant-owner').find('#email-address0').clone();
            clone.attr('id', 'email-address' + count);
            clone.find('[name=company0]').val('').attr('name', 'company' + count);
            clone.find('[for=company0]').val('').attr('for', 'company' + count);
            clone.find('[name=tenant_contact0]').val('').attr('name', 'tenant_contact' + count);               
            clone.find('[for=tenant_contact0]').val('').attr('for', 'tenant_contact' + count);
            clone.insertBefore('#buttons');
            $('#email_container').find('[name=company' + count + ' ]').rules("add", { required: true, messages: { required: "Please enter a company name" } });
            $('#email_container').find('[name=tenant_contact' + count + ']').rules("add", { email: true, required: true, messages: { email: "Please enter a valid email", required: "Please enter a email" } });

            if (count == 1) {
                $('#add_delete').append('<a class="delete" style="text-decoration:none; font-weight:normal; font-size:12px;top:-24px;color:#5F5F5F!important;display:inline-block;width:120px;position:relative;" href="#"><img src="/images/minus.png" style="padding-top:0;"/> Delete Email</a>');
                $('a.delete').bind('click', function (event) {
                    event.preventDefault();
                    if (count != 1) {
                        count--;
                        $('#email_container').find('#email-address' + count).remove();
                    }
                    if (count == 1) {
                        $('a.delete').remove();
                    }
                });
            }
            count++;

            return false;
        });
        if (self.buildingOwner.Tenants == null) {
            self.buildingOwner.Tenants = [];
            self.buildingOwner.Companies = [];
        }
        else {
            var tenants = self.buildingOwner.Tenants;
            var companies = self.buildingOwner.Companies;
            for (var i = 0; i < tenants.length; i++) {
                if (i == 0) {
                    $('#dashboard-setup-tenant-owner').find('[name=company0]').val(companies[i] == null ? "" : companies[i]);
                    $('#dashboard-setup-tenant-owner').find('[name=tenant_contact0]').val(tenants[i]);
                }
                else {
                    $('#dashboard-setup-tenant-owner').find('[name=addTenant]').trigger('click');
                    $('#dashboard-setup-tenant-owner').find('[group=companies]').last().val(companies[i] == null ? "" : companies[i]);
                    $('#dashboard-setup-tenant-owner').find('[group=tenant_contact]').last().val(tenants[i]);
                }
            }
        }

        $('#dashboard-setup-tenant-owner').find('[name=back]').click(function (event) {
            event.preventDefault();
            self.ParkingLotInformationOverview();
            return false;
        });

        $('#dashboard-setup-tenant-owner').find('[name=continue]').click(function (event) {
            event.preventDefault();
            if ($('#email-address0').find('[name=tenant_contact0]').val() != '') {  //custom validation email block
                $('#email-address0').find('[name=company0]').rules("add", { required: true, messages: { required: "Please enter a company name" } });
            } else {
                if ($('#email_container').find('[name=tenant_contact1]').val() != undefined) {
                    $('#email-address0').find('[name=company0]').rules("add", { required: true, messages: { required: "Please enter a company name" } });
                    $('#email-address0').find('[name=tenant_contact0]').rules("add", { email: true, required: true, messages: { email: "Please enter a valid email", required: "Please enter a email" } });
                } else {
                    $('#email-address0').find('[name=tenant_contact0]').rules("remove");
                    $('#email-address0').find('[name=company0]').rules("remove");
                    $('#email-address0').find('[name=company0]').val("");
                }
            }

            if ($('#dashboard-setup-tenant-owner-form').valid()) {
                self.buildingOwner.Tenants = [];
                self.buildingOwner.Companies = [];
                if ($('#dashboard-setup-tenant-owner').find('[name=tenant_contact]').val() != '') {
                    $('#dashboard-setup-tenant-owner').find('[group=companies]').each(function () {
                        self.buildingOwner.Companies[self.buildingOwner.Companies.length] = $(this).val().trim();
                    });
                    $('#dashboard-setup-tenant-owner').find('[group=tenant_contact]').each(function () {
                        self.buildingOwner.Tenants[self.buildingOwner.Tenants.length] = $(this).val().trim();
                    });
                }

                var model = new Object();
                model.Companies = self.buildingOwner.Companies.join('$');
                model.Emails = self.buildingOwner.Tenants.join(' ');
                self.SendInformationToServer(window.sendTenantUrl, model, function () {
                    self.VerifyOwner();
                });
            }
            return false;
        });
    };

    this.VerifyOwner = function () {
        self.LoadTemplate("ajax/dashboard-setup-verify-owner.html", null);

        var owner = self.buildingOwner;
        self.ReplaceElement('#dashboard-setup-verify-owner', 'buildingName', owner.Building.BuildingName);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'buildingAddress', owner.Building.BuildingAddress.Address1);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'buildingCity', owner.Building.BuildingAddress.City);

        if (owner.Building.BuildingAddress.State == undefined && owner.Building.BuildingAddress.StateID > 0) {
            $.ajax({
                url: window.listStatesUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                async: true,
                success: function (response) {
                    jQuery.each(response, function () {
                        if (this.StateID == owner.Building.BuildingAddress.StateID) {
                            owner.Building.BuildingAddress.State = this.StateCode;
                            self.ReplaceElement('#dashboard-setup-verify-owner', 'buildingState', owner.Building.BuildingAddress.State);
                            return false;
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                }
            });
        }
        else {
            self.ReplaceElement('#dashboard-setup-verify-owner', 'buildingState', owner.Building.BuildingAddress.State);
        }

        self.ReplaceElement('#dashboard-setup-verify-owner', 'buildingZip', owner.Building.BuildingAddress.ZipCode);

        self.ReplaceElement('#dashboard-setup-verify-owner', 'ownerName', owner.OwnerContact.Name);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'ownerFirstName', owner.OwnerContact.FirstName);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'ownerLastName', owner.OwnerContact.LastName);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'ownerEmail', owner.OwnerContact.Email);

        if (owner.OwnerContact.PhoneType == undefined && owner.OwnerContact.PhoneTypeID > 0) {
            $.ajax({
                url: window.ListPhoneTypesUrl,
                type: "POST",
                dataType: "json",
                async: true,
                cache: self.cacheEnable,
                success: function (Response) {
                    jQuery.each(Response, function () {
                        if (this.PhoneTypeID == owner.OwnerContact.PhoneTypeID) {
                            owner.OwnerContact.PhoneType = this.PhoneType1;
                            self.ReplaceElement('#dashboard-setup-verify-owner', 'ownerPhoneType', owner.OwnerContact.PhoneType);
                            return false;
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
        }
        else {
            self.ReplaceElement('#dashboard-setup-verify-owner', 'ownerPhoneType', owner.OwnerContact.PhoneType);
        }

        self.ReplaceElement('#dashboard-setup-verify-owner', 'ownerPhone', owner.OwnerContact.PhoneNumber);

        self.ReplaceElement('#dashboard-setup-verify-owner', 'propertyName', owner.ParkingManagementContact.Name);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'propertyFirstName', owner.ParkingManagementContact.FirstName);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'propertyLastName', owner.ParkingManagementContact.LastName);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'propertyEmail', owner.ParkingManagementContact.Email);

        if (owner.ParkingManagementContact.PhoneType == undefined && owner.ParkingManagementContact.PhoneTypeID > 0) {
            $.ajax({
                url: window.ListPhoneTypesUrl,
                type: "POST",
                dataType: "json",
                async: true,
                cache: self.cacheEnable,
                success: function (Response) {
                    jQuery.each(Response, function () {
                        if (this.PhoneTypeID == owner.ParkingManagementContact.PhoneTypeID) {
                            owner.ParkingManagementContact.PhoneType = this.PhoneType1;
                            self.ReplaceElement('#dashboard-setup-verify-owner', 'propertyPhoneType', owner.ParkingManagementContact.PhoneType);
                            return false;
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
        }
        else {
            self.ReplaceElement('#dashboard-setup-verify-owner', 'propertyPhoneType', owner.ParkingManagementContact.PhoneType);
        }

        self.ReplaceElement('#dashboard-setup-verify-owner', 'propertyPhone', owner.ParkingManagementContact.PhoneNumber);

        self.ReplaceElement('#dashboard-setup-verify-owner', 'bankName', owner.Building.BankAccount.BankName);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'bankHolderName', owner.Building.BankAccount.NameOnAccount);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'bankAccountNumber', owner.Building.BankAccount.CheckingAccountNumber);
        self.ReplaceElement('#dashboard-setup-verify-owner', 'bankRoutingNumber', owner.Building.BankAccount.RoutingNumber);

        var handlerDescription;
        switch (owner.HandlerType) {
            case 1:
                handlerDescription = "Property Management";
                break;
            case 2:
                handlerDescription = "Parking Management";
                break;
            case 3:
                handlerDescription = "Building Owner";
                break;
        }
        self.ReplaceElement('#dashboard-setup-verify-owner', 'handling', handlerDescription);

        self.ReplaceElement('#dashboard-setup-verify-owner', 'parkingManagerSame', owner.ParkingManagementContact.Same ? "Yes" : "No");

        var tenantTemplate = $('#dashboard-setup-verify-owner').find('[name=tenantTemplate]');
        var before = tenantTemplate;
        if (owner.Tenants.length == 0 || owner.Tenants[0]== '') {
            $('#dashboard-setup-verify-owner').find('[name=tenantTemplate]').text();
        } else {
            for (var i = 0; i < owner.Tenants.length; i++) {
                var item = tenantTemplate.clone();
                item.find('[name="tenantNumber"]').replaceWith((i + 1).toString());
                item.find('[name="tenantEmail"]').replaceWith(owner.Companies + '/' + owner.Tenants[i]);
                before.after(item);
                before = item;
            }
        }
        tenantTemplate.remove();

        $('#dashboard-setup-verify-owner').find('[name=editBuilding]').click(function (event) {
            event.preventDefault();
            self.BuildingInformation(true);
            return false;
        });

        $('#dashboard-setup-verify-owner').find('[name=editOwner]').click(function (event) {
            event.preventDefault();
            self.OwnershipInformation(true);
            return false;
        });

        $('#dashboard-setup-verify-owner').find('[name=editPropertyManagement]').click(function (event) {
            event.preventDefault();
            self.PropertyManagementInformation(true);
            return false;
        });

        $('#dashboard-setup-verify-owner').find('[name=editParkingManagement]').click(function (event) {
            event.preventDefault();
            self.ParkingManagementInformation(true);
            return false;
        });

        $('#dashboard-setup-verify-owner').find('[name=editBanking]').click(function (event) {
            event.preventDefault();
            self.BankAccountSetupInformation(true);
            return false;
        });

        $('#dashboard-setup-verify-owner').find('[name=editParking]').click(function (event) {
            event.preventDefault();
            self.ParkingLotInformation(true);
            return false;
        });

        $('#dashboard-setup-verify-owner').find('[name=editTenants]').click(function (event) {
            event.preventDefault();
            self.TenantInformation();
            return false;
        });

        $('#dashboard-setup-verify-owner').find('[name=back]').click(function (event) {
            event.preventDefault();
            self.TenantInformation();
            return false;
        });

        $('#dashboard-setup-verify-owner').find('[name=continue]').click(function (event) {
            event.preventDefault();

            self.SendInformationToServer(window.saveBuildingOwnerUrl, null, function () {
                self.NewBuildingAddedCongratulations();
            });
            return false;
        });
    };

    jQuery.validator.addMethod("notEqual", function (value, element, param) {
        return this.optional(element) || value !== param;
    }, '');

    this.PropertyManagementNotifications = function (callback) {
        $.ajax({
            url: "ajax/property-management-notified.html",
            type: "GET",
            dataType: "html",
            cache: self.cacheEnable,
            success: function (html) {
                $('#overlay').css('z-index', '102');
                $('#overlay').show();
                $('body').append(html);
                $('#property-management-notified').center();

                $('.close').click(function (forgotCloseEvent) {
                    forgotCloseEvent.preventDefault();
                    $('#overlay').css('z-index', '100');
                    $('.pop-up').filter('#property-management-notified').remove();
                    if (callback != null) {
                        callback();
                    }
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });
    };

    this.NewBuildingAddedCongratulations = function () {
        self.LoadTemplate("ajax/dashboard-setup-congratulations-owner.html", null);

        $('#dashboard-setup-congratulations-owner').find('[name=finish]').click(function (event) {
            event.preventDefault();
            self.NewBuildingAddedNotifications(true);
            return false;
        });
    };

    this.NewBuildingAddedNotifications = function () {
        self.LoadTemplate("ajax/dashboard-setup-notification-owner.html", null);

        $('#notification').find('[name=close]').click(function (event) {
            event.preventDefault();
            window.location.href = window.dashboardUrl;
            return false;
        });
    };

    this.LoadTemplate = function(file, callback) {
        $.ajax({
            url: file,
            type: "GET",
            dataType: "html",
            async: false,
            cache: self.cacheEnable,
            success: function (html) {
                $('#overlay').show();
                $('.pop-up').remove();
                $('body').append(html);
                if (callback != null) {
                    callback(false);
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });
    };

    this.SendInformationToServer = function(address, model, callback) {
        $.ajax({
            url: address,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            data: model,
            success: function (response) {
                if (response.success) {
                    callback();
                } else {
                    alert(response.error);
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });
    };

    this.RestoreState = function(container, field, value) {
        return $(container).find('[name="' + field + '"]').val(value);
    };

    this.ReplaceElement = function(container, field, value) {
        return $(container).find('[name="' + field + '"]').replaceWith(value);
    };
}