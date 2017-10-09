$(document).ready(function () {
    var index = new Index();
    index.Start(); 
});

var profileData;
var parkingData;
var accountData;
var vehicleData;
var paymentType = '';
var onlineCheck;
var creditCard;

var companyInfo;
var companyPerson;
var personally = true;
var companyPaying;
var emails = [];
var names = [];
var paying = [];

function Index() {
    var self = this;
    this.cacheEnable = false;
    this.common = new Common();
    this.my_addition = My_addition();

    this.Start = function () {
        var qString = window.location.search.substring(0);
        if (qString == '') {
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });
        }
        else {
            qString = qString.replace("?","");
            var vars = qString.split("=");
            if (vars[0] != 'conf') {
                $.backstretch("/images/home-bg.jpg", { speed: 'slow' });
            }
            //reset password
            if (vars[0] == 'reset') {
                $('#login').hide();
                qString = qString.replace("reset=", "");

                $.ajax({
                    url: "./Account/ResetPassword",
                    type: "POST",
                    data: {
                        "qString": qString,
                        "check": true
                    },
                    dataType: "json",
                    success: function (response) {
                        if (response.success) {
                            $.ajax({
                                url: "ajax/passwordreset.html",
                                type: "GET",
                                dataType: "html",
                                success: function (html) {
                                    $('#overlay').show();
                                    $('#login').hide();
                                    $('body').append(html);

                                    $("#password-reset-form").validate({
                                        rules: {
                                            password: {
                                                required: true,
                                                minlength: 6,
                                            },
                                            password2: {
                                                required: true,
                                                minlength: 6,
                                                equalTo: "#newpassword",
                                            },
                                        },
                                        messages: {
                                            password: { required: "Too short", minlength: "Too short" },
                                            password2: { required: "Do not match", minlength: "Do not match", equalTo: "Do not match" }
                                        }
                                    });

                                    $('#password-reset-form').submit(function (event) {
                                        event.preventDefault();
                                        $('#password-reset-form').validate();

                                        if ($("#password-reset-form").valid()) {
                                            $.ajax({
                                                url: "./Account/ResetPassword",
                                                type: "POST",
                                                data: {
                                                    "qString": qString,
                                                    "password": $(this).find('input[name="password"]').val(),
                                                    "check": false
                                                    //  "password2": $(this).find('input[name="password2"]').val()
                                                },
                                                dataType: "json",
                                                success: function (response) {
                                                    if (response.success) {
                                                        $.ajax({
                                                            url: "/ajax/confirmresetpassword.html",
                                                            type: "GET",
                                                            dataType: "html",
                                                            success: function (html) {
                                                                $('#password-reset').remove();
                                                                $('#overlay').show();
                                                                $('body').append(html);
                                                            },
                                                            error: function (XMLHttpRequest, etype, exo) {
                                                                self.common.ShowErrorBox(etype, exo);
                                                            }
                                                        });
                                                        event.preventDefault();
                                                    }
                                                    else {
                                                        self.common.ShowErrorBox(etype, exo);
                                                    }
                                                },
                                                error: function (XMLHttpRequest, etype, exo) {
                                                    self.common.ShowErrorBox(etype, exo);
                                                }
                                            });
                                        }
                                    });
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        }
                        else {
                            $.ajax({
                                url: "/ajax/expiredresetpassword.html",
                                type: "GET",
                                dataType: "html",
                                success: function (html) {
                                    $('#overlay').show();
                                    $('#login').hide();
                                    $('body').append(html);
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        }
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            //register employee
            if (vars[0] == 'reg') {
                $('#login').hide();
                qString = qString.replace("reg=", "");

                $.ajax({
                    url: "./Account/ValidRegisterRef",
                    type: "POST",
                    data: {
                        "queryString": qString
                    },
                    dataType: "json",
                    success: function (response) {
                        if (response.success) {
                            $.ajax({
                                url: "ajax/signup.html",
                                type: "GET",
                                dataType: "html",
                                cache: false,
                                success: function (html) {
                                    $('body').append(html);
                                    var heightSignUp = $("#sign-up").height() + 100;
                                    var heightWrapper = $("#wrapper").height();
                                    var height = (heightSignUp > heightWrapper ? heightSignUp : heightWrapper);
                                    $('#overlay').height(height);
                                    $('#overlay').show();
                                    $('#backstretch').remove();
                                    $.backstretch("/images/signup-bg.jpg", { speed: 'slow' });

                                    $('#signup-form').find('#tenantId').val(response.tenantId);
                                    $('#signup-form').find('[name="email"]').val(response.email);
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        }
                        else {                            
                            self.common.ShowErrorBox('', response.error);
                            $('#login').show();
                        }
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
        }
        
        self.OnLogin();
        self.OnForgotUsername();
        self.OnForgotPassword();
    };

    this.CenteredPopup = function() {
        var wleft = (screen.width / 2) - ($('.dashboard-setup').width() / 2);
        var wtop = (screen.height / 2) - ($('.dashboard-setup').height() / 2);
        $('.dashboard-setup').css({ 'margin-top': wtop, 'margin-left': wleft, top:0, left:0, });
    };

    jQuery.fn.center = function() {
        this.css("position", "absolute");
        this.css("top", (jQuery(window).height() - this.height()) / 2 + jQuery(window).scrollTop() + "px");
        this.css("left", (jQuery(window).width() - this.width()) / 2 + jQuery(window).scrollLeft() + "px");
        this.css({ 'margin-top': 0, 'margin-left': 0, });
        return this;
    };

    this.OnForgotPassword = function () {

        $.validator.setDefaults({
            debug: true
        });

        $('#forgot-password-link').click(function (event) {
            $.ajax({
                url: "ajax/forgotpassword.html",
                type: "GET",
                dataType:"html",
                success: function (html) {
                    $('#overlay').show();
                    $('body').append(html);
                    $('#overlay').css('z-index', '102');
                    $('.close').click(function(forgotCloseEvent){
                        forgotCloseEvent.preventDefault();
                        $('#overlay').css('z-index', '100');
                        $('#overlay').hide();
                        $('.pop-up').filter(':not(#login)').remove();
                        $('#login').show();
                        return false;
                    });

                    $("#forgot-password-form").validate({
                        rules: {
                            email: {
                                required: true,
                                email: true,
                            },
                        },
                        messages: {
                            email: { required: "Invalid", email: "Invalid" }
                        }
                    });

                    $('#forgot-password-form').submit(function(forgotSendEvent){
                        forgotSendEvent.preventDefault();
                        $('#forgot-password-form').validate();

                        if ($("#forgot-password-form").valid()) {
                            $.ajax({
                                url: "./Account/ForgotPassword",
                                type: "POST",
                                data: {
                                    "email": $(this).find('input[name="email"]').val()
                                },
                                dataType: "json",
                                success: function (response) {
                                    if (response.success) {
                                        $.ajax({
                                            url: "ajax/forgotpasswordsent.html",
                                            type: "GET",
                                            dataType: "html",
                                            success: function (html) {
                                                $('#forgot-password').remove();
                                                $('#overlay').show();
                                                $('body').append(html);
                                            },
                                            error: function (XMLHttpRequest, etype, exo) {
                                                self.common.ShowErrorBox(etype, exo);                                            
                                            }
                                        });
                                    }
                                    else {
                                        $('input[name="email"]').parent().append("<label id = 'nosuch' for='email' generated='true' class='error'>" + response.error + "<label>");
                                        $('#nosuch').delay(3000).fadeOut(500, function () { $('#nosuch').remove(); });
                                    }
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        }
                        return false;
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            event.preventDefault();
        });
    };

    this.OnForgotUsername = function () {

        $('#forgot-username-link').click(function (event) {
            $.ajax({
                url: "ajax/forgotusername.html",
                type: "GET",
                dataType: "html",
                success: function (html) {
                    $('#overlay').css('z-index', '102');
                    $('#overlay').show();
                    $('body').append(html);

                    $('.close').click(function (forgotCloseEvent) {
                        forgotCloseEvent.preventDefault();
                        $('#overlay').css('z-index', '100');
                        $('#overlay').hide();
                        $('.pop-up').filter(':not(#login)').remove();
                        $('#login').show();
                        return false;
                    });

                    $('#forgot-username-form').validate({
                        rules: {
                            email: {
                                required: true,
                                email: true,
                            },
                        },
                        messages: {
                            email: { required: "Invalid", email: "Invalid" }
                        }
                    });

                    $('#forgot-username-form').submit(function (event) {
                        $('#forgot-username-form').validate();

                        if ($("#forgot-username-form").valid()) {
                            $.ajax({
                                url: "./Account/ForgotUsername",
                                type: "POST",
                                data: {
                                    "email": $(this).find('input[name="email"]').val()
                                },
                                dataType: "json",
                                success: function (response) {
                                    //window.location.href = "/";//
                                    if (response.success) {
                                        $.ajax({
                                            url: "ajax/forgotusernamesend.html",
                                            type: "GET",
                                            dataType: "html",
                                            success: function (html) {
                                                $('#forgot-username').remove();
                                                $('#overlay').show();
                                                $('body').append(html);

                                            },
                                            error: function (XMLHttpRequest, etype, exo) {
                                                alert(etype, exo);
                                            }
                                        });
                                    } else {
                                        $('input[name="email"]').parent().append("<label id = 'nosuch' for='email' generated='true' class='error'>" + response.error + "<label>");
                                    }
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        }
                        event.preventDefault();
                        return false;
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            event.preventDefault();
        });
    };

    this.OnLogin = function () {
        $('#loginform').submit(function (event) {
            event.preventDefault();           
            var Login = new Object();
            Login.UserName = $(this).find('input[name="username"]').val();
            Login.Password = $(this).find('input[name="password"]').val();
            Login.RememberMe = $(this).find('label[for="remember"]').hasClass('c_on');
            $.ajax({
                url: window.loginUrl,
                type: "POST",
                dataType: "json",
                data: Login,
                success: function (loginResponse) {
                    var step;
                    if (loginResponse.success) {
                        if (loginResponse.Redirect != '') {
                            window.location.href = loginResponse.Redirect;
                            return false;
                        };
                        if (loginResponse.UserStatus == 5) {

                            //if (loginResponse.RegistrationStep > 0) {
                            step = self.GetRegistrationData();
                            // }
                            if (step >= 13 && step <= 21) {
                                return false;
                            }

                            var startUrl = "ajax/dashboard-setup-getting-started.html";
                            var completeFunction = self.GetStarted();
                            switch (step) {
                                case 1:
                                    startUrl = "ajax/dashboard-setup-profile.html";
                                    completeFunction = self.SetProfile();
                                    break;
                                case 2:
                                    startUrl = "ajax/dashboard-setup-parking.html";
                                    break;
                                case 3:
                                    startUrl = "ajax/dashboard-setup-account.html";
                                    break;
                                case 4:
                                    startUrl = "ajax/dashboard-setup-vehicle.html";
                                    break;
                                case 5:
                                    startUrl = "ajax/dashboard-setup-payment.html";
                                    break;
                                case 6: 
                                    startUrl = "ajax/dashboard-setup-payment4.html";
                                    break;
                                case 7: //tenant branch
                                    startUrl = "ajax/dashboard-setup-company2-tenant.html";
                                    break;
                                case 8:
                                    startUrl = "ajax/dashboard-setup-company7-tenant.html";
                                    break;
                                case 9:
                                    startUrl = "ajax/dashboard-setup-parking-tenant.html";
                                    break;
                                case 10:
                                    startUrl = "ajax/dashboard-setup-parking-vehicle2-tenant.html";
                                    break;
                                case 11:
                                    startUrl = "ajax/dashboard-setup-payment2-tenant.html";
                                    break;
                                case 12:
                                    startUrl = "ajax/dashboard-setup-payment4-tenant.html";
                                    break;
                                default:
                            }
                            $.ajax({
                               url: startUrl,
                                type: "GET",
                                dataType: "html",
                                cache: self.cacheEnable,
                                success: function (html) {
                                    $('#login').hide();
                                    $('#overlay').show();
                                    $('body').append(html);
                                    switch (step) {
                                        case 1:
                                            self.SetProfile();
                                            break;
                                        case 2:
                                            self.SetParking(false);
                                            break;
                                        case 3:
                                            self.SetAccount(false);
                                            break;
                                        case 4:
                                            self.SetVehicle(false, false);
                                            break;
                                        case 5:
                                            self.SetPayment();
                                            break;
                                        case 6:
                                            self.SetPayment4(false);
                                            break;
                                        case 7:
                                            self.SetCompany(false);
                                            break;
                                        case 8:
                                            self.SetCompany2(false);
                                            break;
                                        case 9:
                                            self.SetLeaseParking();
                                            break;
                                        case 10:
                                            self.SetVehicle(true);
                                            break;
                                        case 11:
                                            self.SetPayment1(false);
                                            break;
                                        case 12:
                                            self.SetPayment4(true);
                                            break;
                                        default:
                                            self.GetStarted();
                                            break;
                                    }
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        };
                    } else {
                        $.ajax({
                            url: "ajax/loginerror.html",
                            type: "GET",
                            dataType: "html",
                            success: function (html) {
                                $('#overlay').css('z-index', '102');
                                $('#overlay').show();
                                $('body').append(html);
                                $('#login-error').find('p').text(loginResponse.error);
                            },
                            error: function (XMLHttpRequest, etype, exo) {
                                self.common.ShowErrorBox(etype, exo);
                            }
                        });
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                    return;
                }
            });

            return false;
        });
    };

    this.GetRegistrationData = function () {
        var step = 0;
        var countStep = 0;
        $.ajax({
            url: window.getRegistrationDataUrl,
            type: "POST",
            dataType: "json",
            async: false,
            cache: self.cacheEnable,
            success: function (response) {
                if (response.success) {
                    step = response.data.Step;
                    countStep = step;
                    var start_date = '';
                    var end_date = '';
                    if (step > 0 && step < 7) {
                        for (var i = 1; i < countStep; i++) {
                            step--;
                            switch (step) {
                                case 1:
                                    profileData = new Object();
                                    profileData.ProfileTypeId = response.data.ProfileTypeID == null ? '' : response.data.ProfileTypeID;
                                    profileData.BuildingToLotID = response.data.BuildingToLotID == null ? '' : response.data.BuildingToLotID;
                                    profileData.LotId = response.data.LotID == null ? '' : response.data.LotID;
                                    profileData.TenantId = response.data.TenantID == null ? '' : response.data.TenantID;
                                    break;
                                case 2:
                                    parkingData = new Object();
                                    if (response.data.Employee.ParkingInventory.EffectiveFrom != null) {
                                        start_date = new Date(parseInt(response.data.Employee.ParkingInventory.EffectiveFrom.slice(6, -2)));
                                        start_date = start_date.getMonth() + "/" + start_date.getDate() + "/" + start_date.getFullYear();
                                    }
                                    parkingData.StartDate = start_date;
                                    parkingData.ReservedSpaces = response.data.Employee.ParkingInventory.ReservedSpaces == null ? '' : response.data.Employee.ParkingInventory.ReservedSpaces;
                                    parkingData.ReservedSpacesCost = response.data.Employee.ParkingInventory.ReservedSpacesCost == null ? '' : '$' + response.data.Employee.ParkingInventory.ReservedSpacesCost;
                                    if (response.data.Employee.ParkingInventory.EffectiveTo != null) {
                                        end_date = new Date(parseInt(response.data.Employee.ParkingInventory.EffectiveTo.slice(6, -2)));
                                        end_date = end_date.getMonth() + "/" + end_date.getDate() + "/" + end_date.getFullYear();
                                    }
                                    parkingData.EndDate = end_date;
                                    parkingData.UnReservedSpaces = response.data.Employee.ParkingInventory.UnReservedSpaces == null ? '' : response.data.Employee.ParkingInventory.UnReservedSpaces;
                                    parkingData.UnReservedSpacesCost = response.data.Employee.ParkingInventory.UnReservedSpacesCost == null ? '' : '$' + response.data.Employee.ParkingInventory.UnReservedSpacesCost;
                                    break;
                                case 3:
                                    accountData = new Object();
                                    accountData.PhoneTypeId = response.data.Employee.Phone.PhoneTypeID == null ? '' : response.data.Employee.Phone.PhoneTypeID;
                                    accountData.PhoneNumber = response.data.Employee.Phone.PhoneNumber == null ? '' : response.data.Employee.Phone.PhoneNumber;
                                    accountData.FirstName = response.data.Employee.Profile.FirstName;
                                    accountData.LastName = response.data.Employee.Profile.LastName;
                                    accountData.EmailAddress = response.data.Employee.Profile.EmailAddress;
                                    break;
                                case 4:
                                    vehicleData = new Object();
                                    vehicleData.MakeId = response.data.Employee.Vehicle.VehicleMakeID == null ? '' : response.data.Employee.Vehicle.VehicleMakeID;
                                    vehicleData.ModelId = response.data.Employee.Vehicle.VehicleModelID == null ? '' : response.data.Employee.Vehicle.VehicleModelID;
                                    vehicleData.Year = response.data.Employee.Vehicle.Year == null ? '' : response.data.Employee.Vehicle.Year;
                                    vehicleData.LicenseNumber = response.data.Employee.Vehicle.LicensePlateNumber == null ? '' : response.data.Employee.Vehicle.LicensePlateNumber;
                                    vehicleData.Color = response.data.Employee.Vehicle.Color == null ? '' : response.data.Employee.Vehicle.Color;
                                    vehicleData.PermitNumber = response.data.Employee.Vehicle.PermitNumber == null ? '' : response.data.Employee.Vehicle.PermitNumber;
                                    break;
                                case 5:
                          //          paymentData=
                                break;
                                //default:
                            }
                        }
                    }
                    if (step > 6 && step < 13) {
                        for (var i = 6; i < countStep; i++) {
                            step--;
                            switch (step) {
                                case 6:
                                    profileData = new Object();
                                    profileData.ProfileTypeId = response.data.ProfileTypeID == null ? '' : response.data.ProfileTypeID;
                                    profileData.BuildingToLotID = response.data.BuildingToLotID == null ? '' : response.data.BuildingToLotID;
                                    profileData.LotId = response.data.LotID == null ? '' : response.data.LotID;
                                    break;
                                case 7:
                                    companyInfo = new Object();
                                    companyInfo.CompanyName = response.data.Tenant.Company.CompanyName == null ? '' : response.data.Tenant.Company.CompanyName;
                                    companyInfo.Suite = response.data.Tenant.Company.Suite == null ? '' : response.data.Tenant.Company.Suite;
                                    companyInfo.asBuilding = response.data.Tenant.Company.asBuilding == null ? '' : response.data.Tenant.Company.asBuilding;
                                    companyInfo.City = response.data.Tenant.Company.Address.City == null ? '' : response.data.Tenant.Company.Address.City;
                                    companyInfo.Address1 = response.data.Tenant.Company.Address.Address1 == null ? '' : response.data.Tenant.Company.Address.Address1;
                                    companyInfo.Address2 = response.data.Tenant.Company.Address.Address2 == null ? '' : response.data.Tenant.Company.Address.Address2;
                                    companyInfo.StateId = response.data.Tenant.Company.Address.StateID == null ? '' : response.data.Tenant.Company.Address.StateID;
                                    companyInfo.ZipCode = response.data.Tenant.Company.Address.ZipCode == null ? '' : response.data.Tenant.Company.Address.ZipCode;
                                    break;
                                case 8:
                                    companyPerson = new Object();
                                    companyPerson.FirstName = response.data.Tenant.ManagerProfile.FirstName == null ? '' : response.data.Tenant.ManagerProfile.FirstName;
                                    companyPerson.LastName = response.data.Tenant.ManagerProfile.LastName == null ? '' : response.data.Tenant.ManagerProfile.LastName;
                                    companyPerson.EmailAddress = response.data.Tenant.ManagerProfile.EmailAddress == null ? '' : response.data.Tenant.ManagerProfile.EmailAddress;
                                    companyPerson.isManager = response.data.Tenant.isManager == null ? '' : response.data.Tenant.isManager;
                                    companyPerson.isMailing = response.data.Tenant.isMailing == null ? '' : response.data.Tenant.isMailing;
                                    companyPerson.PhoneTypeId = response.data.Tenant.ManagerPhone.PhoneTypeID == null ? '' : response.data.Tenant.ManagerPhone.PhoneTypeID;
                                    companyPerson.PhoneNumber = response.data.Tenant.ManagerPhone.PhoneNumber == null ? '' : response.data.Tenant.ManagerPhone.PhoneNumber;
                                    companyPerson.City = response.data.Tenant.ManagerAddress.City == null ? '' : response.data.Tenant.ManagerAddress.City;
                                    companyPerson.Address1 = response.data.Tenant.ManagerAddress.Address1 == null ? '' : response.data.Tenant.ManagerAddress.Address1;
                                    companyPerson.Address2 = response.data.Tenant.ManagerAddress.Address2 == null ? '' : response.data.Tenant.ManagerAddress.Address2;
                                    companyPerson.StateId = response.data.Tenant.ManagerAddress.StateID == null ? '' : response.data.Tenant.ManagerAddress.StateID;
                                    companyPerson.ZipCode = response.data.Tenant.ManagerAddress.ZipCode == null ? '' : response.data.Tenant.ManagerAddress.ZipCode;
                                    break;
                                case 9:
                                    parkingData = new Object();
                                    personally = response.data.Tenant.personally;
                                    if (response.data.Tenant.ParkingInventory.EffectiveFrom != null) {
                                        start_date = new Date(parseInt(response.data.Tenant.ParkingInventory.EffectiveFrom.slice(6, -2)));
                                        start_date = start_date.getMonth() + "/" + start_date.getDate() + "/" + start_date.getFullYear();
                                    }
                                    parkingData.StartDate = start_date;
                                    parkingData.ReservedSpaces = response.data.Tenant.ParkingInventory.ReservedSpaces == null ? '' : response.data.Tenant.ParkingInventory.ReservedSpaces;
                                    parkingData.ReservedSpacesCost = response.data.Tenant.ParkingInventory.ReservedSpacesCost == null ? '' : '$' + response.data.Tenant.ParkingInventory.ReservedSpacesCost;
                                    if (response.data.Tenant.ParkingInventory.EffectiveTo != null) {
                                        end_date = new Date(parseInt(response.data.Tenant.ParkingInventory.EffectiveTo.slice(6, -2)));
                                        end_date = end_date.getMonth() + "/" + end_date.getDate() + "/" + end_date.getFullYear();
                                    }
                                    parkingData.EndDate = end_date;
                                    parkingData.UnReservedSpaces = response.data.Tenant.ParkingInventory.UnReservedSpaces == null ? '' : response.data.Tenant.ParkingInventory.UnReservedSpaces;
                                    parkingData.UnReservedSpacesCost = response.data.Tenant.ParkingInventory.UnReservedSpacesCost == null ? '' : '$' + response.data.Tenant.ParkingInventory.UnReservedSpacesCost;
                                    break;
                                case 10:
                                    vehicleData = new Object();
                                    vehicleData.MakeId = response.data.Tenant.Vehicle.VehicleMakeID == null ? '' : response.data.Tenant.Vehicle.VehicleMakeID;
                                    vehicleData.ModelId = response.data.Tenant.Vehicle.VehicleModelID == null ? '' : response.data.Tenant.Vehicle.VehicleModelID;
                                    vehicleData.Year = response.data.Tenant.Vehicle.Year == null ? '' : response.data.Tenant.Vehicle.Year;
                                    vehicleData.Color = response.data.Tenant.Vehicle.Color == null ? '' : response.data.Tenant.Vehicle.Color;
                                    vehicleData.LicenseNumber = response.data.Tenant.Vehicle.LicensePlateNumber == null ? '' : response.data.Tenant.Vehicle.LicensePlateNumber;
                                    vehicleData.PermitNumber = response.data.Tenant.Vehicle.PermitNumber == null ? '' : response.data.Tenant.Vehicle.PermitNumber;
                                    break;
                                case 11:
                                    if (response.data.Tenant.EmailString == null || response.data.Tenant.EmailString == '') {
                                        companyPaying = false;
                                    }
                                    else {
                                        companyPaying = true;
                                        emails = response.data.Tenant.EmailString.split(" ");
                                        names = response.data.Tenant.NameString.split("$");
                                        paying = response.data.Tenant.PayingString.split("$");
                                    }
                                    break;
                                //default:
                            }
                        }
                    }
                    if (step >= 13 && step <= 21) {
                        profileData = new Object();
                        profileData.ProfileTypeId = response.data.ProfileTypeID == null ? '' : response.data.ProfileTypeID;
                        self.owner.Restore(step, response.data.Owner);
                    }
                }
                else
                {                    
                    self.common.ShowErrorBox('', response.error);
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });

        return countStep;
    };

    this.GetStarted = function () {
        $('#backstretch').remove();
        // self.CenteredPopup();
        $('.dashboard-setup').center();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });

        $('#dashboard-setup-start').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-start').find('[name=cancel]').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });
    
        $('#dashboard-setup-start').find('[name=continue]').click(function (event) {
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
                    self.SetProfile();
                    
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });
    };

    this.SetProfile = function () {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });

        $('#select_tenant').hide();
        $('#select_parking').hide();

        $('.dashboard-setup').center();

        $.ajax({
            url: window.listUserProfileTypeUrl,
            type: "POST",
            async: false,
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                jQuery.each(response, function () {
                    $('[name=profile_type]').append("<option value='" + this.UserProfileTypeID + "'>" + this.UserProfileType1 + "</option>");
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });

        $.ajax({
            url: window.listLotsUrl,
            type: "POST",
            async: false,
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                jQuery.each(response, function () {
                    $('[name=parking_type]').append("<option value='" + this.BuildingToLotID + "'>" + this.Lot.LotName + "/" + this.Building.BuildingName + "</option>");
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });

        $.ajax({
            url: window.listTenantsUrl,
            type: "POST",
            async: false,
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                jQuery.each(response, function () {
                    $('[name=tenant_empl]').append("<option value='" + this.CompanyID + "'>" + this.CompanyName + "</option>");
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });

        $('#dashboard-setup-profile').find('[name=profile_type]').change(function (event) {
            if (this.value != '') {
                if (this.value == 3) {
                    $('#select_tenant').show();               
                    $('#select_parking').show();
                }
                else if (this.value == 5) {
                    $('#select_parking').hide();
                    $('#select_tenant').hide();
                }
                else {
                    $('#select_parking').show();
                    $('#select_tenant').hide();
                    $('[name=tenant_empl]').val('');
                }
            }
            else {
                $('#select_parking').hide();
                $('#select_tenant').hide();
                $('[name=tenant_empl]').val('');
            }
            $('.dashboard-setup').center();
            if (profileData != null && this.value != profileData.ProfileTypeId) {
                resetCachedInfo();
            }
            return false;
        });

        if (profileData != null) {
            $('[name=profile_type]').val(profileData.ProfileTypeId);
            $('[name=profile_type]').bindOptions();
            $('[name=profile_type]').change();
            $('[name=parking_type]').val(profileData.BuildingToLotID);
            $('[name=parking_type]').bindOptions();
            //$('#select_parking').show();
            $('[name=tenant_empl]').val(profileData.TenantId == 0 ? '' : profileData.TenantId);
        }
        else {
            $('[name=profile_type]').bindOptions();
        }
        $('[name=parking_type]').bindOptions();
        $('[name=tenant_empl]').bindOptions();

        $('#dashboard-setup-profile').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-profile').find('[name=cancel]').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-profile').find('[name=continue]').click(function (event) {
            event.preventDefault();
            var tenantVal = $('#dashboard-setup-profile').find('[name=tenant_empl]').val();
            var tenantId = (tenantVal == '' ? 0 : tenantVal);
            var profileType = $('[name=profile_type]').val();
            var parkingType = $('[name=parking_type]').val();

            profileData = new Object();
            profileData.ProfileTypeId = profileType;
            profileData.BuildingToLotID = parkingType;
            profileData.TenantId = tenantId;

            profileData.BuildingToLotName = $('[name=parking_type] option:selected').text();
            profileData.TenantName = (tenantVal == '' ? '' : $('#dashboard-setup-profile').find('[name=tenant_empl] option:selected').text());

            if (profileType == '') {
                $.ajax({
                    url: "ajax/profileerror.html",
                    type: "GET",
                    dataType: "html",
                    success: function (html) {
                        $('#overlay').css('z-index', '102');
                        $('#overlay').show();
                        $('body').append(html);
                        //$('#profile-error').center();
                        $('#profile-error').find('h3').text('Error!');
                        $('#profile-error').find('p').text('Please select your profile type');                        
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });

                return false;
            }

            if (parkingType == '' && profileType != 5) {
                $.ajax({
                    url: "ajax/profileerror.html", 
                    type: "GET",
                    dataType: "html",
                    success: function (html) {
                        $('#overlay').css('z-index', '102');
                        $('#overlay').show();
                        $('body').append(html);

                        $('#profile-error').center();
                        $('#profile-error').find('h3').text('Error!');
                        $('#profile-error').find('p').text('Please select parking lot');
                    },
                    error: function (XMLHttpRequest, etype, exo) {                        
                        self.common.ShowErrorBox(etype, exo);                        
                    }
                });

                return false;
            }
            if (parkingType == '') {
                parkingType = 0;
                profileData.BuildingToLotID = parkingType;
            }

            $.ajax({
                url: window.setProfileTypeUrl,
                type: "POST",
                dataType: "json",
                data: profileData,

                success: function (setProfileTypeResponse) {
                    if (setProfileTypeResponse.success) {
                        profileData.LotId = setProfileTypeResponse.lotId;
                        if (profileData.ProfileTypeId==3) {
                            $.ajax({
                                url: (tenantId == '' ? "ajax/dashboard-setup-parking.html" : "ajax/dashboard-employer-notified.html"),
                                type: "GET",
                                dataType: "html",
                                cache: self.cacheEnable,
                                success: function (html) {
                                    $('#overlay').show();
                                    $('.pop-up').filter(':not(#login)').remove();
                                    $('body').append(html);
                                    tenantId == '' ? self.SetParking(false) : self.EmplNotified();
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        }
                        else if (profileData.ProfileTypeId == 5) { // Building Owner
                            self.owner.Start();
                        }
                        else {
                            $.ajax({
                                url: "ajax/dashboard-setup-company2-tenant.html",
                                type: "GET",
                                dataType: "html",
                                cache: self.cacheEnable,
                                success: function (html) {
                                    $('#overlay').show();
                                    $('.pop-up').filter(':not(#login)').remove();
                                    $('body').append(html);
                                    self.SetCompany(false);
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        }
                    } else {                        
                        self.common.ShowErrorBox('Error', setProfileTypeResponse.error);
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                    return;
                }
            });
            return false;
        });
    };

    this.EmplNotified = function () {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
        $('#employer-notified').center();
        

        $('#employer-notified').find('.close').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-parking.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetParking(false);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });
    };

    this.SetParking = function (isVerify) {
        var reservedSpace;
        var unreservedSpace;

        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
        // $('.dashboard-setup').center();

        if (profileData.LotId != 0) {

            $.ajax({
                url: window.getParkingInfoUrl,
                type: "POST",
                async: false,
                dataType: "json",
                data: {
                    "lotId": profileData.LotId
                },
                cache: self.cacheEnable,
                success: function (response) {
                    reservedSpace = response.stall.ReservedSpace;
                    unreservedSpace = response.stall.UnreservedSpace;
                    $('[name=reserved_spaces_cost]').val('$' + response.stall.ReservedRate);
                    $('[name=unreserved_spaces_cost]').val('$' + response.stall.UnreservedRate);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }
        for (var i = 0; i < reservedSpace; i++) {
            $('[name=reserved_spaces]').append("<option value='" + (i + 1) + "'>" + (i + 1) + "</option>");
        }
        for (var i = 0; i < unreservedSpace; i++) {
            $('[name=unreserved_spaces]').append("<option value='" + (i + 1) + "'>" + (i + 1) + "</option>");
        }

        if (parkingData != null) {
            $('#start_date').val(parkingData.StartDate);
            $('[name=reserved_spaces]').val(parkingData.ReservedSpaces);
            
            $('#end_date').val(parkingData.EndDate);
            $('[name=unreserved_spaces]').val(parkingData.UnReservedSpaces);
        }

        $('[name=reserved_spaces]').bindOptions();
        $('[name=unreserved_spaces]').bindOptions();

        $('#dashboard-setup-parking').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-parking').find('[name=back]').click(function (event) {
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
                    self.SetProfile();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });
    
        $('#dashboard-setup-parking').find('[name=continue]').click(function (event) {
            event.preventDefault();

            var parkingInfo = new Object();
            parkingData = new Object();
            parkingInfo.LotId = profileData.LotId;
            parkingInfo.StartDate = parkingData.StartDate = $('#start_date').val();
            parkingInfo.ReservedSpaces = parkingData.ReservedSpaces = ($('[name=reserved_spaces]').val() == '' ? 0 : $('[name=reserved_spaces]').val());
            parkingInfo.EndDate = parkingData.EndDate = $('#end_date').val();
            parkingInfo.UnReservedSpaces = parkingData.UnReservedSpaces = ($('[name=unreserved_spaces]').val() == '' ? 0 : $('[name=unreserved_spaces]').val());
            parkingInfo.isTenant = false;
            parkingInfo.ReservedSpacesCost = parkingData.ReservedSpacesCost = $('#reserved_spaces_cost').val().replace('$', '');
            parkingInfo.UnReservedSpacesCost = parkingData.UnReservedSpacesCost = $('#unreserved_spaces_cost').val().replace('$', '');

            $.ajax({
                url: window.setParkingInfoUrl,
                type: "POST",
                dataType: "json",
                data: parkingInfo,
                success: function (setParking) {
                    if (setParking.success) {
                        $.ajax({
                            url: "ajax/dashboard-setup-account.html",
                            type: "GET",
                            dataType: "html",
                            cache: self.cacheEnable,
                            success: function (html) {
                                $('#overlay').show();
                                $('.pop-up').filter(':not(#login)').remove();
                                $('body').append(html);
                                if (isVerify) {
                                    self.GetVerify(false);
                                } else {
                                    self.SetAccount(false);
                                }
                            },
                            error: function (XMLHttpRequest, etype, exo) {
                                self.common.ShowErrorBox(etype, exo);
                            }
                        });
                    }
                    else {
                        alert(setParking.error);
                        self.common.ShowErrorBox('Error', setParking.error);
                    }
                        
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                    return;
                }
            });
            return false;
        });
    };

    this.SetAccount = function (isVerify) {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
     //   $('.dashboard-setup').center();

        $.ajax({
            url: window.ListPhoneTypesUrl,
            type: "POST",
            dataType: "json",
            async: true,
            cache: self.cacheEnable,
            success: function (Response) {
                jQuery.each(Response,function() {
                    $('#phone_select').append("<option value='" + this.PhoneTypeID + "'>" + this.PhoneType1 + "</option>");
                });
                if (accountData != null) {
                    $('#phone_select').val(accountData.PhoneTypeId);
                }
                $('#phone_select').bindOptions();
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
                return;
            }
        });
        if (accountData == null) {
            $.ajax({
                url: window.getPersonalData,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                success: function (response) {
                    $('.pop-up').find('[name=first_name]').val(response.firstName != null ? response.firstName : '');
                    $('.pop-up').find('[name=last_name]').val(response.lastName != null ? response.lastName : '');
                    $('.pop-up').find('[name=email]').val(response.EmailAddress != null ? response.EmailAddress : '');
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }

        if (accountData != null) {
            $('#phone_number').val(accountData.PhoneNumber);
            $('.pop-up').find('[name=first_name]').val(accountData.FirstName);
            $('.pop-up').find('[name=last_name]').val(accountData.LastName);
            $('.pop-up').find('[name=email]').val(accountData.EmailAddress);
        }

        $('#dashboard-setup-account').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });
            return false;
        });
        
        $('#dashboard-setup-account').find('[name=back]').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-parking.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetParking(false);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });

        $.validator.addMethod("phoneformat", 
                              function(value, element) {
                                  return value.match(/\(\d\d\d\)\d\d\d\-\d\d\d\d$/);                               
                                      }, 
                             "Should be the format (000)000-0000"
                             );

        $("input#phone_number").mask("(999)999-9999");
        
        $('#dashboard-setup-account-form').validate({
            rules: {
                email: {
                    required: true,
                    email: true,
                },
                device: {
                    required: true
                },
                phone: {
                    required: true,
                    phoneformat: true,
                },
            },
            messages: {
                email: { required: "Enter email", email: "Incorrect email format" },
                device: { required: "Select device" },
                phone: { required: "Enter telephone number"}
            }
        });

        $('#dashboard-setup-account').find('[name=continue]').click(function (event) {
            event.preventDefault();
            if ($('#dashboard-setup-account-form').valid()) {
                accountData = new Object();
                accountData.PhoneTypeId = $('#phone_select').val();
                accountData.PhoneNumber = $('#phone_number').val();
                accountData.PhoneTypeName = $('#phone_select option:selected').text();
                accountData.FirstName = $('.pop-up').find('[name=first_name]').val();
                accountData.LastName = $('.pop-up').find('[name=last_name]').val();
                accountData.EmailAddress = $('.pop-up').find('[name=email]').val();

                $.ajax({
                    url: window.setPersonalData,
                    type: "POST",
                    dataType: "json",
                    data: {
                        "firstName": $('.pop-up').find('[name=first_name]').val(),
                        "lastName": $('.pop-up').find('[name=last_name]').val(),
                        "EmailAddress": $('.pop-up').find('[name=email]').val(),
                        "phoneTypeId": $('#phone_select').val(),
                        "phoneNumber": $('#phone_number').val(),
                    },

                    success: function (response) {
                        if (response.success) {
                            $.ajax({
                                url: "ajax/dashboard-setup-vehicle.html",
                                type: "GET",
                                dataType: "html",
                                cache: self.cacheEnable,
                                success: function (html) {
                                    $('#overlay').show();
                                    $('.pop-up').filter(':not(#login)').remove();
                                    $('body').append(html);
                                    if (isVerify) {
                                        self.GetVerify(false);
                                    } else {
                                        self.SetVehicle(false, false);
                                    }
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.common.ShowErrorBox(etype, exo);
                                }
                            });
                        } else {                            
                            self.common.ShowErrorBox('Error', setPersonalAddressResponse.error);
                        }
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                        return;
                    }
                });
            }
            return false;
        });
    };

    this.SetVehicle = function (isTenant, isVerify) {
        var makes;
        var models;
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
    //    $('.dashboard-setup').center();

        if (vehicleData != null) {
            $('[name=color]').val(vehicleData.Color);
            $('[name=lnum]').val(vehicleData.LicenseNumber);
            $('[name=pnum]').val(vehicleData.PermitNumber);

            $.ajax({
                url: window.listMakesUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                success: function (response) {
                    makes = response;
                    make = $('[name=vehicle_make]');
                    model = $('[name=vehicle_model]');
                    year = $('[name=vehicle_year]'); 
                    make.append("<option value=''>Please select a make</option>"); 
                    makeList = make.parents('.jquery-selectbox').find('.jquery-selectbox-list');
                    for (i = 0; i < makes.length; i++) {
                        make.append("<option value='" + makes[i].VehicleMakeID + "'>" + makes[i].VehicleMake1 + "</option>");
                    }
                    model.append("<option value=''>Please select a model</option>");
                    year.append("<option value=''>Please select a year</option>");
                    
                    make.val(vehicleData.MakeId);

                    if (make.val() != '') {
                        self.GetModels(make.val());
                        
                        model.val(vehicleData.ModelId);
                    
                        if (model.val()!='') {
                            jQuery.each(models,function() {
                                if(this.VehicleModelID==model.val())
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
                                if (models[i].VehicleMakeID==make.val() && jQuery.inArray(models[i].Year1,rez)==-1) {
                                    rez.push(models[i].Year1);
                                    year.append("<option value='" + models[i].Year1 + "'>" + models[i].Year1 + "</option>");
                                } 
                            }
                        } 
                   
                        $('[name=vehicle_year]').val(vehicleData.Year);             
                    } 
                    make.bindOptions();
                    model.bindOptions();
                    year.bindOptions();  
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }
        else {
            $.ajax({
                url: window.listMakesUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                success: function (response) {
                    makes = response;
                    make = $('[name=vehicle_make]');
                    model = $('[name=vehicle_model]');
                    year = $('[name=vehicle_year]'); 
                    make.append("<option value=''>Please select a make</option>"); 
                    makeList = make.parents('.jquery-selectbox').find('.jquery-selectbox-list');
                    for (i = 0; i < makes.length; i++) {
                        make.append("<option value='" + makes[i].VehicleMakeID + "'>" + makes[i].VehicleMake1 + "</option>");
                    }
                    model.append("<option value=''>Please select a model</option>");
                    year.append("<option value=''>Please select a year</option>");
                
                    make.bindOptions();
                    model.bindOptions();
                    year.bindOptions();
                    
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }


        $('#dashboard-setup-vehicle').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-vehicle').find('[name=back]').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: (isTenant ? "ajax/dashboard-setup-parking-vehicle1-tenant.html" : "ajax/dashboard-setup-account.html"),
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    isTenant ? self.PersonallyParking(isVerify) : self.SetAccount(false);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    alert(etype, exo);
                }
            });
            return false;
        });

        $('[name=vehicle_make]').change(function() {
            makeId = $('[name=vehicle_make]').val();
            model = $('[name=vehicle_model]');
            model.empty();
            year = $('[name=vehicle_year]'); 
            year.empty();
            model.append("<option value=''>Please select a model</option>");
            year.append("<option value=''>Please select a year</option>");          
            if (makeId != '') {
                self.GetModels(makeId); 
            }
            else {
                model.bindOptions();
                year.bindOptions();
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
            year.bindOptions(); 
        });

        this.GetModels = function (makeId) {
            $.ajax({
                url: window.listModelsUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                async: false,
                data: {
                    "makeId": makeId,
                },
                success: function (response) {
                    models=response;
                    if (models.length!=0) {
                        self.InsertModels(makeId);
                    }
                    else {
                        model.bindOptions();
                        year.bindOptions();
                    }  
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });  
        };

        this.InsertModels = function (makeId) {
            var rez=[];
            var rez2=[];           
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
            model.bindOptions();
            year.bindOptions();
        };
    
        $('#dashboard-setup-vehicle').find('[name=continue]').click(function (event) {
            event.preventDefault();

            var vehicleModel = new Object();
            vehicleData = new Object();
            vehicleModel.isTenant = isTenant;
            vehicleModel.VehicleMakeID = vehicleData.MakeId = $('.pop-up').find('[name=vehicle_make]').val();
            vehicleModel.VehicleModelID = vehicleData.ModelId = $('.pop-up').find('[name=vehicle_model]').val();
            vehicleModel.Year = vehicleData.Year = $('.pop-up').find('[name=vehicle_year]').val() != null ? $('.pop-up').find('[name=vehicle_year]').val() : '';
            vehicleModel.LicensePlateNumber = vehicleData.LicenseNumber = $('.pop-up').find('[name=lnum]').val();
            vehicleModel.Color = vehicleData.Color = $('.pop-up').find('[name=color]').val();
            vehicleModel.PermitNumber = vehicleData.PermitNumber = $('.pop-up').find('[name=pnum]').val();

            vehicleData.Make = vehicleData.MakeId == '' ? '' : $('.pop-up').find('[name=vehicle_make] option:selected').text();
            vehicleData.Model = vehicleData.ModelId == '' ? '' : $('.pop-up').find('[name=vehicle_model] option:selected').text();

            $.ajax({
                url: window.newVehicleUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: vehicleModel,

                success: function (newVehicleResponse) {
                    $.ajax({
                        url: (isTenant ? "ajax/dashboard-setup-payment2-tenant.html" : "ajax/dashboard-setup-payment.html"),
                        type: "GET",
                        dataType: "html",
                        cache: self.cacheEnable,
                        success: function (html) {
                            $('#overlay').show();
                            $('.pop-up').filter(':not(#login)').remove();
                            $('body').append(html);
                            if (isVerify) {
                                self.GetVerify(isTenant);
                            } else {
                                isTenant ? self.SetPayment1(false) : self.SetPayment();
                            }
                        },
                        error: function (XMLHttpRequest, etype, exo) {
                            self.common.ShowErrorBox(etype, exo);
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                    return;
                }
            });
            return false;
        });
    };

    this.SetPayment = function () {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
        $('.dashboard-setup').center();

        if (companyPaying != null) {
            $("input:radio[name=payment][value=" + (companyPaying ? 2 : 1) + "]").prop('checked', 'checked');
            setupLabel();
        }

        $('#dashboard-setup-payment').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });
        
        $('#dashboard-setup-payment').find('[name=back]').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-vehicle.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetVehicle(false, false);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });

        $('#dashboard-setup-payment').find('[name=payment]').click(function (event) {
            if ($('[name=payment]').filter($("#tenant")).is(":checked")) {
                $('#footnote').css('color', '#ff0000');
                $('#footnote').css('line-height', '17px');
                companyPaying = true;
            }
            else {
                $('#footnote').css('color', '#5F5F5F');
                $('#footnote').css('line-height', '14px');
                companyPaying = false;
            }
        });

        $('#dashboard-setup-payment').find('[name=continue]').click(function (event) {
            event.preventDefault();

           // companyPaying = $('input:radio[name=payment]:checked]').val() == 2;

            $.ajax({
                url: "ajax/dashboard-setup-payment4.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetPayment4(false);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });
    };

    this.SetPayment4 = function (isTenant) {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
        //self.CenteredPopup();

        var init = true;
        var homeAddress = [6];

        $('#credit_card').hide();
        $('#online_check').hide();
        $('#cards').hide();
        $('.li_address').hide();
        $('.dashboard-setup').center();

        $("input[name=card_number]").mask("9999 9999 9999 9999");
        $('input[name=routing_number]').mask("999999999");

        $("#online_check").validate({
            rules: {
                routing_number: {
                    routignumber: true,
                },
            },
            messages: {
                routing_number: {},
            },
        });
        
        $("#credit_card").validate({
            rules: {
                card_number: {
                    cardformat: true,
                },
            },
            messages: {
                card_number: {},
            },
        });
        
        $('[name=payment_method]').change(function () {
            $('#credit_card').hide();
            $('#online_check').hide();
            $('#cards').hide();
           // $("input[name=card_number]").rules("remove");
            if ($('[name=payment_method]').val() != '') {
                block = '#' + $('[name=payment_method]').val();
                $(block).show();
                if ($('[name=payment_method]').val() == 'credit_card') {
                    $("input[name=card_number]").rules("add", { required: true, messages: { required: "Should be the format 0000 0000 0000 0000" } });
                    $('#cards').css("display","inline");
                    if (init) {
                        init = false;
                        var d = new Date();

                        for (var i = 0; i < 10; i++) {
                            $('[name=year]').append("<option value='" + (d.getFullYear() + i) + "'>" + (d.getFullYear() + i) + "</option>")
                        }
                        $('[name=year]').bindOptions();

                        $.ajax({
                            url: window.listStatesUrl,
                            type: "POST",
                            dataType: "json",
                            cache: self.cacheEnable,
                            async: false,
                            success: function (response) {
                                jQuery.each(response, function () {
                                    $('[name=state]').append("<option value='" + this.StateID + "'>" + this.StateCode + "</option>");
                                });
                                $('[name=state]').bindOptions();
                            },
                            error: function (XMLHttpRequest, etype, exo) {
                                self.common.ShowErrorBox(etype, exo);
                            }
                        });
                    }
                }
            }
            $('.dashboard-setup').center();
        });

        if (paymentType != '') {
            $('[name=payment_method]').val(paymentType);
            $('[name=payment_method]').bindOptions();
            $('[name=payment_method]').change();
            if (onlineCheck != null) {
                $('.pop-up').find('[name=account_name]').val(onlineCheck.AccountName);
                $('input:radio[name="checking_type"]').filter('[value="' + onlineCheck.CheckingTypeID + '"]').attr('checked', true).parent().addClass('r_on');
                $('.pop-up').find('[name=account_number]').val(onlineCheck.AccountNumber1);
                $('.pop-up').find('[name=account_number2]').val(onlineCheck.AccountNumber2);
                $('.pop-up').find('[name=bank_name]').val(onlineCheck.BankName);
                $('.pop-up').find('[name=routing_number]').val(onlineCheck.RoutingNumber);
            }
            if (creditCard != null) {
                $('.pop-up').find('[name=addressId]').val(creditCard.AddressId);
                $('.pop-up').find('[name=ch_first_name]').val(creditCard.CHFirstName);
                $('.pop-up').find('[name=ch_last_name]').val(creditCard.CHLastName);
                $('.pop-up').find('[name=card_number]').val(creditCard.CardNumber);
                $('.pop-up').find('[name=month]').val(creditCard.Month).bindOptions();
                $('.pop-up').find('[name=year]').val(creditCard.Year).bindOptions();
                $('.pop-up').find('[name=cvv]').val(creditCard.CVV);
                $('.pop-up').find('[name=auto_pay]').attr("checked", creditCard.AutoPay).parent().css('background-image', (creditCard.AutoPay == true ? 'url("../images/check-on.png")' : 'url("../images/check-off.png")'));
                $('.pop-up').find('[name=billing_address]').attr("checked", creditCard.BillingAddress).parent().css('background-image', (creditCard.BillingAddress == true ? 'url("../images/check-on.png")' : 'url("../images/check-off.png")'));
                $('.li_address').show();
                $('.pop-up').find('[name=address1]').val(creditCard.Address1);
                $('.pop-up').find('[name=address2]').val(creditCard.Address2);
                $('.pop-up').find('[name=city]').val(creditCard.City);
                $('.pop-up').find('[name=state]').val(creditCard.StateId).bindOptions();
                $('.pop-up').find('[name=zip]').val(creditCard.ZipCode);
            }
        }

        $('#dashboard-setup-payment3').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-payment3').find('[name=back]').click(function (event) {
            event.preventDefault();
            if ($('#credit_card').is(':visible') || $('#online_check').is(':visible')) {
                $('[name=payment_method]').val('').bindOptions();
                $('#credit_card').hide();
                $('#cards').hide();
                $('#online_check').hide();
                $('.dashboard-setup').center();
            }
            else {    
                paymentType = $('[name=payment_method]').val();
                onlineCheck = new Object();
                onlineCheck.AccountName = $('.pop-up').find('[name=account_name]').val();
                onlineCheck.CheckingTypeID = $('input:radio[name="checking_type"]').filter(":checked").val();
                onlineCheck.AccountNumber1 = $('.pop-up').find('[name=account_number]').val();
                onlineCheck.AccountNumber2 = $('.pop-up').find('[name=account_number2]').val();
                onlineCheck.BankName = $('.pop-up').find('[name=bank_name]').val();
                onlineCheck.RoutingNumber = $('.pop-up').find('[name=routing_number]').val();
                creditCard = new Object();
                creditCard.AddressId = $('.pop-up').find('[name=addressId]').val();
                creditCard.CHFirstName = $('.pop-up').find('[name=ch_first_name]').val();
                creditCard.CHLastName = $('.pop-up').find('[name=ch_last_name]').val();
                creditCard.CardNumber = $('.pop-up').find('[name=card_number]').val();
                creditCard.Month = $('.pop-up').find('[name=month]').val();
                creditCard.Year = $('.pop-up').find('[name=year]').val();
                creditCard.CVV = $('.pop-up').find('[name=cvv]').val();
                creditCard.AutoPay = ($('.pop-up').find('[name=auto_pay]').attr("checked") == "checked" ? true : false);
                creditCard.BillingAddress = ($('.pop-up').find('[name=billing_address]').attr("checked") == "checked" ? true : false);
                creditCard.Address1 = $('.pop-up').find('[name=address1]').val();
                creditCard.Address2 = $('.pop-up').find('[name=address2]').val();
                creditCard.City = $('.pop-up').find('[name=city]').val();
                creditCard.StateId = $('.pop-up').find('[name=state]').val();
                creditCard.ZipCode = $('.pop-up').find('[name=zip]').val();

                $.ajax({
                    url: (isTenant ? "ajax/dashboard-setup-payment2-tenant.html" : "ajax/dashboard-setup-payment.html"),
                    type: "GET",
                    dataType: "html",
                    cache: self.cacheEnable,
                    success: function (html) {
                        $('.pop-up').filter(':not(#login)').remove();
                        $('body').append(html);
                        $('#overlay').show();
                        isTenant ? self.SetPayment1(false) : self.SetPayment();
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            return false;
        });

        $('#auto_pay').click(function (event) {
            if ($('[name=auto_pay]').attr("checked") != "checked") {
                $('[name=auto_pay]').attr("checked", "checked");
                $(this).css('background-image', 'url("../images/check-on.png")');
            }
            else {
                $(this).css('background-image', 'url("../images/check-off.png")');
                $('[name=auto_pay]').removeAttr("checked");
            }
        });

        $('#billing_address').click(function (event) {
            if ($('[name=billing_address]').attr("checked") != "checked") {
                $('.li_address').show();
                $('[name=billing_address]').attr("checked", "checked");
                $(this).css('background-image', 'url("../images/check-on.png")');
                if (isTenant) {
                    if (homeAddress.length == 1) {
                        $.ajax({
                            url: isTenant == false ? window.getAddressUrl : window.getCompanyAddressUrl,
                            type: "POST",
                            dataType: "json",
                            asinc: false,
                            cache: self.cacheEnable,
                            success: function (getAddressResponse) {
                                if (getAddressResponse.success) {
                                    homeAddress[0] = getAddressResponse.address.AddressID;
                                    homeAddress[1] = getAddressResponse.address.Address1;
                                    homeAddress[2] = getAddressResponse.address.Address2;
                                    homeAddress[3] = getAddressResponse.address.City;
                                    homeAddress[4] = getAddressResponse.address.StateID;
                                    homeAddress[5] = getAddressResponse.address.ZipCode;
                                    SetAddress();
                                }
                                else {
                                    self.common.ShowErrorMessage(getUserProfileResponse.error);
                                }
                            },
                            error: function (XMLHttpRequest, etype, exo) {
                                self.common.ShowErrorBox(etype, exo);
                                return;
                            }
                        });
                    }
                    else {
                        SetAddress();
                    }
                }
            }
            else {
     //           $('.li_address').hide();
                $(this).css('background-image', 'url("../images/check-off.png")');
                $('[name=billing_address]').removeAttr("checked");
                $('[name=addressId]').val('');
                $('[name=address1]').val('');
                $('[name=address2]').val('');
                $('[name=city]').val('');
                $('[name=state]').val('');
                $('[name=zip]').val('');
                $('[name=state]').val('').bindOptions();
            }
            $('.dashboard-setup').center();
        });

        function SetAddress() {
            $('[name=addressId]').val(homeAddress[0]);
            $('[name=address1]').val(homeAddress[1]);
            $('[name=address2]').val(homeAddress[2]);
            $('[name=city]').val(homeAddress[3]);
            $('[name=state]').val(homeAddress[4]);
            $('[name=zip]').val(homeAddress[5]);
            $('[name=state]').bindOptions();
        };

        $('#dashboard-setup-payment3').find('[name=finish]').click(function(event) {
            event.preventDefault();

            var payment_method = paymentType = $('.pop-up').find('[name=payment_method]').val();

            //if user does not have the type of payment
            if (payment_method == '') {
                self.GetVerify(isTenant);
                return false;
            }

            if (payment_method == "online_check") {
                if ($('#online_check').valid()) {
                    var onlineCheckModel = new Object();
                    onlineCheck = new Object();
                    onlineCheckModel.NameOnAccount = onlineCheck.AccountName = $('.pop-up').find('[name=account_name]').val();
                    onlineCheckModel.OnlineCheckingTypeID = onlineCheck.CheckingTypeID = $('input:radio[name="checking_type"]').filter(":checked").val();
                    onlineCheckModel.CheckingAccountNumber = onlineCheck.AccountNumber1 = onlineCheck.AccountNumber2 = $('.pop-up').find('[name=account_number]').val();
                    onlineCheckModel.BankName = onlineCheck.BankName = $('.pop-up').find('[name=bank_name]').val();
                    onlineCheckModel.RoutingNumber = onlineCheck.RoutingNumber = $('.pop-up').find('[name=routing_number]').val();
                    onlineCheckModel.isTenant = isTenant;
                    self.savePayment(payment_method, onlineCheckModel, isTenant);
                }
            } else {
                if (payment_method == "credit_card") {
                    if ($('#credit_card').valid()) {
                        var creditCardModel = new Object();
                        creditCard = new Object();
                        //creditCardModel.AddressId = $('.pop-up').find('[name=addressId]').val();
                        creditCardModel.CHFirstName = creditCard.CHFirstName = $('.pop-up').find('[name=ch_first_name]').val();
                        creditCardModel.CHLastName = creditCard.CHLastName = $('.pop-up').find('[name=ch_last_name]').val();
                        creditCardModel.CardNumber = creditCard.CardNumber = $('.pop-up').find('[name=card_number]').val();
                        creditCardModel.ExpDateMount = creditCard.Month = $('.pop-up').find('[name=month]').val();
                        creditCardModel.ExpDateYear = creditCard.Year = $('.pop-up').find('[name=year]').val();
                        creditCardModel.CVV = creditCard.CVV = $('.pop-up').find('[name=cvv]').val();
                        creditCardModel.AutoPay = creditCard.AutoPay = ($('.pop-up').find('[name=auto_pay]').attr("checked") == "checked" ? true : false);
                        if (isTenant) {
                            creditCardModel.isOffice = creditCard.BillingAddress = ($('.pop-up').find('[name=billing_address]').attr("checked") == "checked" ? true : false);
                        } else {
                            creditCardModel.isHome = creditCard.BillingAddress = ($('.pop-up').find('[name=billing_address]').attr("checked") == "checked" ? true : false);
                        }
                        creditCardModel.Address1 = creditCard.Address1 = $('.pop-up').find('[name=address1]').val();
                        creditCardModel.Address2 = creditCard.Address2 = $('.pop-up').find('[name=address2]').val();
                        creditCardModel.City = creditCard.City = $('.pop-up').find('[name=city]').val();
                        creditCardModel.StateId = creditCard.StateId = $('.pop-up').find('[name=state]').val();
                        creditCardModel.ZipCode = creditCard.ZipCode = $('.pop-up').find('[name=zip]').val();
                        creditCardModel.isTenant = isTenant;

                        creditCard.StateName = $('.pop-up').find('[name=state] option:selected').text();
                        self.savePayment(payment_method, creditCardModel, isTenant);
                    }
                } 
            }
            return false;
        });
    };

    this.savePayment = function (payment_method, model, isTenant) {
        $.ajax({
            url: payment_method == "credit_card" ? newPaymentCCUrl : newPaymentOCUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            data: model,
            success: function (response) {
                if (response.success) {
                    self.GetVerify(isTenant);
                } else {
                    self.common.ShowErrorBox('Error', response.error);
                }

            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
                return;
            }
        });
    };

    this.SetCompany = function (isVerify) {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
        var init = true;
      //  $('#use_buliding_address_yes').attr('checked', 'checked');
        $('#dashboard-setup-company-tenant2').find('.li_address').hide();

        $('.dashboard-setup').center();

        $.ajax({
            url: window.listStatesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            async: false,
            success: function (response) {
                jQuery.each(response, function () {
                    $('[name=state]').append("<option value='" + this.StateID + "'>" + this.StateCode + "</option>");
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });

        if (companyInfo != null) {
            $('.pop-up').find('[name=name]').val(companyInfo.CompanyName);
            $('.pop-up').find('[name=suite_number]').val(companyInfo.Suite);
            $('#dashboard-setup-company-tenant2').find('.li_address').show();
            if (companyInfo.asBuilding == true) {
                init = false;
                $('#use_buliding_address_yes').attr('checked', 'checked');
            }
            else {
                $('#use_buliding_address_no').attr('checked', 'checked');
            }
            $('.dashboard-setup').center();
            $('.pop-up').find('[name=city]').val(companyInfo.City);
            $('.pop-up').find('[name=address1]').val(companyInfo.Address1);
            $('.pop-up').find('[name=address2]').val(companyInfo.Address2);
            $('.pop-up').find('[name=state]').val(companyInfo.StateId);
            $('.pop-up').find('[name=zip]').val(companyInfo.ZipCode);
        }

        setupLabel();
        $('[name=state]').bindOptions();

        $('#dashboard-setup-company-tenant2').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('.label_radio').click(function () {
            setupLabel();
            $('#dashboard-setup-company-tenant2').find('.li_address').show();
            $('.dashboard-setup').center();
            if ($('.label_radio input:checked').val() == 'no') {
                $('#dashboard-setup-company-tenant2').find('.li_address').find('input:text').val('');
            }
            else {
                if (init) {
                    init = false;
                    $.ajax({
                        url: window.getBuildingAddressUrl,
                        type: "POST",
                        dataType: "json",
                        cache: self.cacheEnable,
                        async: false,
                        data: {
                            'buildingToLotID': profileData.BuildingToLotID,
                        },
                        success: function (response) {
                            if (response.success) {
                                companyInfo = new Object();
                                companyInfo.City = response.address.City;
                                companyInfo.Address1 = response.address.Address1;
                                companyInfo.Address2 = response.address.Address2;
                                companyInfo.StateId = response.address.StateID;
                                companyInfo.ZipCode = response.address.ZipCode;
                            }
                            else {                                
                                self.common.ShowErrorBox('', response.error);
                            } 
                        },
                        error: function (XMLHttpRequest, etype, exo) {
                            self.common.ShowErrorBox(etype, exo);
                        }
                    });
                }
                $('.pop-up').find('[name=city]').val(companyInfo.City);
                $('.pop-up').find('[name=address1]').val(companyInfo.Address1);
                $('.pop-up').find('[name=address2]').val(companyInfo.Address2);
                $('.pop-up').find('[name=state]').val(companyInfo.StateId);
                $('.pop-up').find('[name=zip]').val(companyInfo.ZipCode);
            }
            $('[name=state]').bindOptions();
        });

        $('#dashboard-setup-company-tenant2').find('[name=back]').click(function (event) {
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
                    self.SetProfile();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });

        $('#dashboard-setup-company-tenant2').find('[name=continue]').click(function (event) {
            event.preventDefault();
            companyInfo = new Object();
            companyInfo.StateName = $('.pop-up').find('[name=state] option:selected').text();

            $.ajax({
                url: window.setCompany,
                type: "POST",
                dataType: "json",
                data: {
                    'CompanyName' : companyInfo.CompanyName = $('.pop-up').find('[name=name]').val(),
                    'Suite' : companyInfo.Suite = $('.pop-up').find('[name=suite_number]').val(),
                    'asBuilding': companyInfo.asBuilding = $('.label_radio input:checked').val() == 'yes',
                    'City' : companyInfo.City = $('.pop-up').find('[name=city]').val(),
                    'Address1' : companyInfo.Address1 = $('.pop-up').find('[name=address1]').val(),
                    'Address2' : companyInfo.Address2 = $('.pop-up').find('[name=address2]').val(),
                    'StateId' : companyInfo.StateId = $('.pop-up').find('[name=state]').val(),
                    'Zipcode' : companyInfo.ZipCode = $('.pop-up').find('[name=zip]').val(),
                },

                success: function (response) {
                    if (response.success) {
                        $.ajax({
                            url: "ajax/dashboard-setup-company7-tenant.html",
                            type: "GET",
                            dataType: "html",
                            cache: self.cacheEnable,
                            success: function (html) {
                                $('#overlay').show();
                                $('.pop-up').filter(':not(#login)').remove();
                                $('body').append(html);
                                self.SetCompany2(isVerify);
                            },
                            error: function (XMLHttpRequest, etype, exo) {
                                self.common.ShowErrorBox(etype, exo);
                            }
                        });

                    } else {
                        alert(response.error);
                        self.common.ShowErrorBox('Error', response.error);
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                    return;
                }
            });
            return false;
        });
    };

    this.SetCompany2 = function (isVerify) {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
        
        $('#dashboard-setup-company-tenant2').find('.li_address').hide();
        $('#dashboard-setup-company-tenant2').find('.ul_incharge').hide();
        $('.dashboard-setup').center();

        $.ajax({
            url: window.ListPhoneTypesUrl,
            type: "POST",
            dataType: "json",
            async: false,
            cache: self.cacheEnable,
            success: function (Response) {
                jQuery.each(Response, function () {
                    $('#phone_select').append("<option value='" + this.PhoneTypeID + "'>" + this.PhoneType1 + "</option>");
                });
                $('#phone_select').bindOptions();
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
                return;
            }
        });

        $.ajax({
            url: window.listStatesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            async: false,
            success: function (response) {
                jQuery.each(response, function () {
                    $('[name=state]').append("<option value='" + this.StateID + "'>" + this.StateCode + "</option>");
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });

        if (companyPerson != null) {
            $('.pop-up').find('[name=first_name]').val(companyPerson.FirstName);
            $('.pop-up').find('[name=last_name]').val(companyPerson.LastName);
            $('.pop-up').find('[name=email]').val(companyPerson.EmailAddress);
            if (companyPerson.isManager==true) {
                $('#incharge_yes').attr('checked', 'checked');
                $('#dashboard-setup-company-tenant2').find('.ul_incharge').show();
                $('#dashboard-setup-company-tenant2').find('.use_address').hide();
            }
            else {
                $('#incharge_no').attr('checked', 'checked');
                $('#dashboard-setup-company-tenant2').find('.ul_incharge').show();
                $('#dashboard-setup-company-tenant2').find('.use_address').show();
                $('#dashboard-setup-company-tenant2').find('.li_address').show();
                if (companyPerson.isMailing == true) {
                    $('#use-address_yes').attr('checked', 'checked');
                }
                else {
                    $('#use-address_no').attr('checked', 'checked');
                }
            }
            
            setupLabel();
            $('.pop-up').find('#phone_select').val(companyPerson.PhoneTypeId);
            $('.pop-up').find('[name=phone]').val(companyPerson.PhoneNumber);
            $('.pop-up').find('[name=city]').val(companyPerson.City);
            $('.pop-up').find('[name=address1]').val(companyPerson.Address1);
            $('.pop-up').find('[name=address2]').val(companyPerson.Address2);
            $('.pop-up').find('[name=state]').val(companyPerson.StateId);
            $('.pop-up').find('[name=zip]').val(companyPerson.ZipCode);
            $('.dashboard-setup').center();
        }

        $('[name=state]').bindOptions();
        $('#phone_select').bindOptions();

        $('#dashboard-setup-company-tenant2').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-company-tenant2').find('[name=back]').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-company2-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetCompany(false);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });

        $('.isCompany').click(function () {
            $('#dashboard-setup-company-tenant2').find('.ul_incharge').show();
            if ($('.isCompany input:checked').val() == 'no') {
                $('#dashboard-setup-company-tenant2').find('.use_address').show();
                $('#use-address_yes').attr('checked', 'checked');
                setupLabel();
                $('.isMailing').trigger('click');
                $('[name=state]').bindOptions();
            }
            if ($('.isCompany input:checked').val() == 'yes') 
            {
                $('#dashboard-setup-company-tenant2').find('.use_address').hide();
                $('#dashboard-setup-company-tenant2').find('.li_address').hide();
                $('#dashboard-setup-company-tenant2').find('.li_address').find('input:text').val('');
                $.ajax({
                    url: window.getPersonalData,
                    type: "POST",
                    dataType: "json",
                    async: false,
                    cache: self.cacheEnable,
                    success: function (response) {
                        $('.pop-up').find('[name=first_name]').val(response.firstName != null ? response.firstName : '');
                        $('.pop-up').find('[name=last_name]').val(response.lastName != null ? response.lastName : '');
                        $('.pop-up').find('[name=email]').val(response.EmailAddress != null ? response.EmailAddress : '');
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            $('.dashboard-setup').center();
            $('.isCompany').each(function () {
                $(this).removeClass('r_on');
            });
            $('.isCompany input:checked').each(function () {
                $(this).parent('label').addClass('r_on');
            });
        });

        $('.isMailing').click(function () {
            $('#dashboard-setup-company-tenant2').find('.li_address').show();
            if ($('.isMailing input:checked').val() == 'no') {
                $('#dashboard-setup-company-tenant2').find('.li_address').find('input:text').val('');
            }
            else {
                $('.pop-up').find('[name=city]').val(companyInfo.City);
                $('.pop-up').find('[name=address1]').val(companyInfo.Address1);
                $('.pop-up').find('[name=address2]').val(companyInfo.Address2);
                $('.pop-up').find('[name=state]').val(companyInfo.StateId);
                $('.pop-up').find('[name=zip]').val(companyInfo.ZipCode);  
            }
            $('.dashboard-setup').center();
            $('[name=state]').bindOptions();
            $('.isMailing').each(function () {
                $(this).removeClass('r_on');
            });
            $('.isMailing input:checked').each(function () {
                $(this).parent('label').addClass('r_on');
            });
        });

        $.validator.addMethod("phoneformat",
                              function (value, element) {
                                  return value.match(/\(\d\d\d\)\d\d\d\-\d\d\d\d$/);
                              },
                             "Should be the format (000)000-0000"
                             );

        $("input#phone").mask("(999)999-9999");


        $("#dashboard-setup-company-manager").validate({
            rules: {
                email: {
                    required: true,
                    email: true,
                },
                first_name: {
                    required: true,
                },
                last_name: {
                    required: true,
                },
                device: {
                    required: true
                },
                phone: {
                    required: true,
                    phoneformat: true
                },
            },
            messages: {
                email: { required: "Enter email", email: "Incorrect email format" },
                first_name: { required: "Enter first name" },
                last_name: { required: "Enter last name" },
                device: { required: "Select device" },
                phone: { required: "Enter telephone number" }
            }
        });


        $('#dashboard-setup-company-tenant2').find('[name=continue]').click(function (event) {
            event.preventDefault();
            if ($('#dashboard-setup-company-manager').valid()) {
                if ($('.label_radio input:checked').length > 0) {
                    if ($("#dashboard-setup-company-manager").valid()) {
                        companyPerson = new Object();
                        $.ajax({
                            url: window.setCompany2,
                            type: "POST",
                            dataType: "json",
                            data: {
                                'FirstName': companyPerson.FirstName = $('.pop-up').find('[name=first_name]').val(),
                                'LastName': companyPerson.LastName = $('.pop-up').find('[name=last_name]').val(),
                                'emailAddress': companyPerson.EmailAddress = $('.pop-up').find('[name=email]').val(),
                                'isManager': companyPerson.isManager = $('.isCompany input:checked').val() == 'yes',
                                'isMailing': companyPerson.isMailing = $('.isMailing input:checked').val() == 'yes',

                                'PhoneTypeID': companyPerson.PhoneTypeId = $('.pop-up').find('#phone_select').val(),
                                'PhoneNumber': companyPerson.PhoneNumber = $('.pop-up').find('[name=phone]').val(),

                                'City': companyPerson.City = $('.pop-up').find('[name=city]').val(),
                                'Address1': companyPerson.Address1 = $('.pop-up').find('[name=address1]').val(),
                                'Address2': companyPerson.Address2 = $('.pop-up').find('[name=address2]').val(),
                                'StateId': companyPerson.StateId = ($('.pop-up').find('[name=state]').val() == '' ? 0 : $('.pop-up').find('[name=state]').val()),
                                'Zipcode': companyPerson.ZipCode = $('.pop-up').find('[name=zip]').val(),

                            },
                            success: function (response) {
                                if (response.success) {
                                    companyPerson.UserID = response.userId;
                                    companyPerson.PhoneId = response.phoneId;
                                    companyPerson.AddressId = response.addressId;
                                    $.ajax({
                                        url: "ajax/dashboard-setup-parking-tenant.html",
                                        type: "GET",
                                        dataType: "html",
                                        cache: self.cacheEnable,
                                        success: function (html) {
                                            $('#overlay').show();
                                            $('.pop-up').filter(':not(#login)').remove();
                                            $('body').append(html);
                                            if (isVerify) {
                                                self.GetVerify(true);
                                            } else {
                                                self.SetLeaseParking();
                                            }
                                        },
                                        error: function (XMLHttpRequest, etype, exo) {
                                            self.common.ShowErrorBox(etype, exo);
                                        }
                                    });
                                } else {                                    
                                    self.common.ShowErrorBox('Error', response.error);
                                }
                            },
                            error: function (XMLHttpRequest, etype, exo) {
                                self.common.ShowErrorBox(etype, exo);
                                return;
                            }
                        });
                    }
                    else {
                        GetNotification();
                    }
                }
                else {
                    GetNotification();
                }
            }
            return false;
        });
        function GetNotification() {
            $.ajax({
                url: "ajax/dashboard-notification-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').css('z-index', '102');
                    $('#overlay').show();
                    $('body').append(html);
                    $('#notification').center();

                    $('.close').click(function (forgotCloseEvent) {
                        forgotCloseEvent.preventDefault();
                        $('#overlay').css('z-index', '100');
                        $('.pop-up').filter('#notification').remove();
                        return false;
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }
    };

    this.SetLeaseParking = function () {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
       // $('.dashboard-setup').center();

        this.SetTotal = function () {
            var resTotal = $('[name=reserved_spaces]').val() * $('[name=reserved_spaces_cost]').val().replace('$', '');
            var unresTotal = $('[name=unreserved_spaces]').val() * $('[name=unreserved_spaces_cost]').val().replace('$', '');
            $('#total1').val('$' + parseInt(resTotal));
            $('#total2').val('$' + parseInt(unresTotal));
            return false;
        };

        $('#dashboard-setup-parking').find('#months').blur(function () {
            if ($('#start_date').val() != '') {
                if (this.value == 36 || this.value == 60) {
                    var date = $('#start_date').val().split("/")
                    
                    $('#end_date').val(date[0] + '/' + date[1] + '/' + (parseInt(date[2]) + parseInt(this.value / 12)));
                }
            }
        });
            
        if (profileData.LotId != '') {

            $.ajax({
                url: window.getParkingInfoUrl,
                type: "POST",
                async: false,
                dataType: "json",
                data: {
                    "lotId": profileData.LotId
                },
                cache: self.cacheEnable,
                success: function (response) {
                    reservedSpace = response.stall.ReservedSpace;
                    unreservedSpace = response.stall.UnreservedSpace;
                    $('[name=reserved_spaces_cost]').val('$' + response.stall.ReservedRate);
                    $('[name=unreserved_spaces_cost]').val('$' + response.stall.UnreservedRate);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }
        for (var i = 0; i < reservedSpace; i++) {
            $('[name=reserved_spaces]').append("<option value='" + (i + 1) + "'>" + (i + 1) + "</option>");
        }
        for (var i = 0; i < unreservedSpace; i++) {
            $('[name=unreserved_spaces]').append("<option value='" + (i + 1) + "'>" + (i + 1) + "</option>");
        }

        if (parkingData != null) {
            $('#start_date').val(parkingData.StartDate);
            $('[name=reserved_spaces]').val(parkingData.ReservedSpaces);

            $('#end_date').val(parkingData.EndDate);
            $('[name=unreserved_spaces]').val(parkingData.UnReservedSpaces);
            $('#reserved_spaces_cost').val(parkingData.ReservedSpacesCost);
            $('#unreserved_spaces_cost').val(parkingData.UnReservedSpacesCost);
        }

        $('[name=reserved_spaces]').bindOptions();
        $('[name=unreserved_spaces]').bindOptions();

        self.SetTotal();

        $('[name=reserved_spaces]').change(function () {
            self.SetTotal();
        });

        $('[name=unreserved_spaces]').change(function () {
            self.SetTotal();
        });

        $('#dashboard-setup-parking').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-parking').find('[name=back]').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-company7-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetCompany2(false);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });

        $('#dashboard-setup-parking').find('[name=continue]').click(function (event) {
            event.preventDefault();

            var parkingInfo = new Object();
            parkingData = new Object();
            parkingInfo.LotId = profileData.LotId;
            parkingInfo.StartDate = parkingData.StartDate = $('#start_date').val();
            parkingInfo.ReservedSpaces = parkingData.ReservedSpaces = ($('[name=reserved_spaces]').val() == '' ? 0 : $('[name=reserved_spaces]').val());
            parkingInfo.EndDate = parkingData.EndDate = $('#end_date').val();
            parkingInfo.UnReservedSpaces = parkingData.UnReservedSpaces = ($('[name=unreserved_spaces]').val() == '' ? 0 : $('[name=unreserved_spaces]').val());
            parkingInfo.isTenant = true;
            parkingInfo.ReservedSpacesCost = parkingData.ReservedSpacesCost = $('#reserved_spaces_cost').val().replace('$', '');
            parkingInfo.UnReservedSpacesCost = parkingData.UnReservedSpacesCost = $('#unreserved_spaces_cost').val().replace('$', '');

            $.ajax({
                url: window.setParkingInfoUrl,
                type: "POST",
                dataType: "json",
                data: parkingInfo,
                success: function (setParking) {
                    if (setParking.success) {
                        $.ajax({
                            url: "ajax/dashboard-landlord-notified-tenant.html",
                            type: "GET",
                            dataType: "html",
                            cache: self.cacheEnable,
                            success: function (html) { 
                                $('#overlay').css('z-index', '102');
                                $('#overlay').show();
                                $('body').append(html);

                                $('.close').click(function (forgotCloseEvent) {
                                    forgotCloseEvent.preventDefault();
                                    $('#overlay').css('z-index', '100');
                                    $('.pop-up').filter('#landlord-notified').remove();
                                    $.ajax({
                                        url: "ajax/dashboard-setup-parking-vehicle1-tenant.html",
                                        type: "GET",
                                        dataType: "html",
                                        cache: self.cacheEnable,
                                        success: function (html) {
                                            $('#overlay').show();
                                            $('.pop-up').filter(':not(#login)').remove();
                                            $('body').append(html);
                                            self.PersonallyParking(false);
                                        },
                                        error: function (XMLHttpRequest, etype, exo) {
                                            self.common.ShowErrorBox(etype, exo);
                                        }
                                    });
                                    return false;
                                });
                            },
                            error: function (XMLHttpRequest, etype, exo) {
                                self.common.ShowErrorBox(etype, exo);
                            }
                        });  
                    }
                    else {                        
                        self.common.ShowErrorBox('Error', setParking.error);
                    }
                        
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                    return;
                }
            });
            return false;
        });
    };

    this.PersonallyParking = function (isVerify) {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
        $('.dashboard-setup').center();

        if (personally) {
            $('#personal_parking_yes').attr('checked', 'checked');
        }
        else {
            $('#personal_parking_no').attr('checked', 'checked');
        }
        setupLabel();

        $('#dashboard-setup-company-tenant2').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup-company-tenant2').find('[name=back]').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-parking-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetLeaseParking();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });

        $('#dashboard-setup-company-tenant2').find('[name=continue]').click(function (event) {
            event.preventDefault();

            if ($('.label_radio input:checked').length > 0) {

                if ($('.label_radio input:checked').val() == 'yes') {
                    personally = true;
                    $.ajax({
                        url: "ajax/dashboard-setup-parking-vehicle2-tenant.html",
                        type: "GET",
                        dataType: "html",
                        cache: self.cacheEnable,
                        success: function (html) {
                            $('#overlay').show();
                            $('.pop-up').filter(':not(#login)').remove();
                            $('body').append(html);
                            self.SetVehicle(true, isVerify);
                        },
                        error: function (XMLHttpRequest, etype, exo) {
                            self.common.ShowErrorBox(etype, exo);
                        }
                    });
                }
                else {
                    personally = false;
                    $.ajax({
                        url: "ajax/dashboard-setup-payment2-tenant.html",
                        type: "GET",
                        dataType: "html",
                        cache: self.cacheEnable,
                        success: function (html) {
                            $('#overlay').show();
                            $('.pop-up').filter(':not(#login)').remove();
                            $('body').append(html);
                            self.SetPayment1(false);
                        },
                        error: function (XMLHttpRequest, etype, exo) {
                            self.common.ShowErrorBox(etype, exo);
                        }
                    });
                }

                $.ajax({
                    url: window.setPersonally,
                    type: "POST",
                    async: true,
                    dataType: "json",
                    data: {
                        "personally": personally
                    },
                    cache: self.cacheEnable,
                    success: function (response) {

                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            else {
                alert("Please, make your option!");
            }
            return false;
        });
    };

    this.SetPayment1 = function (isVerify) {
        $('#backstretch').remove();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });
        $('#overlay').show();
        $('#dashboard-setup-company-tenant2').find('.ul_email').hide();
        $('.dashboard-setup').center();
        var emailIndex = 1;

        $('.label_check').click(function () {
            setupLabel();
        });

        $('#dashboard-setup-company-tenant2').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('.label_radio').click(function () {
            if ($('.label_radio input:checked').val() == 'yes') {
                $('#dashboard-setup-company-tenant2').find('.ul_email').show();
                $('#email-addresses').find('input[class="email"]').each(function (index) {
                    $(this).rules("add", { required: true, email: true, messages: { required: "*", email: "*" } });
                    //$(this).focus(function () { if (this.value === 'Email') this.value = ''; });
                    //$(this).blur(function () { if (this.value === '') this.value = 'Email'; });
                });
                $('#email-addresses').find('input[class="name"]').each(function (index) {
                    $(this).rules("add", { required: true, /*notEqual: true,*/ messages: { required: "*"/*, notEqual: "*"*/} });
                    //$(this).focus(function () { if (this.value === 'Name') this.value = ''; });
                    //$(this).blur(function () { if (this.value === '') this.value = 'Name'; });
                });
            }
            else {
                $('#dashboard-setup-company-tenant2').find('.ul_email').hide();
                $('#dashboard-setup-company-tenant2').find('#email-addresses').find('input[type="text"]').each(function (index) {
                    $(this).rules("remove");
                });
            }
            setupLabel();
        });

        $('#dashboard-setup-company-tenant2').find('[name=back]').click(function (event) {
            event.preventDefault();
            if (!personally) {
                $.ajax({
                    url: "ajax/dashboard-setup-parking-vehicle1-tenant.html",
                    type: "GET",
                    dataType: "html",
                    cache: self.cacheEnable,
                    success: function (html) {
                        $('#overlay').show();
                        $('.pop-up').filter(':not(#login)').remove();
                        $('body').append(html);
                        self.PersonallyParking(false);
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            else {
                $.ajax({
                    url: "ajax/dashboard-setup-parking-vehicle2-tenant.html",
                    type: "GET",
                    dataType: "html",
                    cache: self.cacheEnable,
                    success: function (html) {
                        $('#overlay').show();
                        $('.pop-up').filter(':not(#login)').remove();
                        $('body').append(html);
                        self.SetVehicle(true, false);
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            
            return false;
        });

        $("#company-emails").validate({
        });

        $('a.add').click(function (event) {
            event.preventDefault();
            $('#email-addresses').append('<br/><input type="text" class="email" name="employee_email[' + emailIndex + ']" />');//value="Email"
            $('#email-addresses').find('input[name="employee_email[' + emailIndex + ']"]').rules("add", { required: true, email: true, messages: { required: "*", email: "*" } });
            //$('#email-addresses').find('input[name="employee_email[' + emailIndex + ']"]').focus(function() { if (this.value==='Email') this.value = ''; });
            //$('#email-addresses').find('input[name="employee_email[' + emailIndex + ']"]').blur(function () { if (this.value === '') this.value = 'Email'; });
            $('#email-addresses').append('<input type="text" class="name" name="name[' + emailIndex + ']" />');//value="Name" 
            $('#email-addresses').find('input[name="name[' + emailIndex + ']"]').rules("add", { required: true,/* notEqual: true,*/ messages: { required: "*"/*, notEqual: "*"*/ } });
            //$('#email-addresses').find('input[name="name[' + emailIndex + ']"]').focus(function () { if (this.value === 'Name') this.value = ''; });
            //$('#email-addresses').find('input[name="name[' + emailIndex + ']"]').blur(function () { if (this.value === '') this.value = 'Name'; });
            $('#email-addresses').append('<label style="float:right;width:105px;" class="label_check" for="are_paying[' + emailIndex + ']">Will you are pay<input type="checkbox" id="are_paying[' + emailIndex + ']" name="are_paying[' + emailIndex + ']"/></label>');
            $('.label_check').last().click(function () { setupLabel(); });
            if (emailIndex == 1) {
                //$('#add_delete').append('<a class="delete" style="font-size:12px;color:#222;text-decoration:none;height:15px;display:block;float:left;position:relative;margin-left:15px;" href="#"><img src="/images/minus.png" style="padding-top:0;"/> Delete Email</a>');
                $('#add_delete').append('<a class="delete" style="font-size:12px;font-weight:normal;color:#5F5F5F;text-decoration:none;display:block;float:left;position:relative;margin-left:15px;" href="#"><img src="/images/minus.png" style="padding-top:0;"/> Delete Email</a>');
                $('a.delete').bind('click', function (event) {
                    event.preventDefault();
                    if (emailIndex != 1) {
                        emailIndex--;
                        $('#email-addresses').find('[name="employee_email[' + emailIndex + ']"]').remove();
                        $('#email-addresses').find('[name="name[' + emailIndex + ']"]').remove();
                        $('#email-addresses').find('[for="are_paying[' + emailIndex + ']"]').remove();
                        $('#email-addresses').find('[for="employee_email[' + emailIndex + ']"]').remove();
                        $('#email-addresses').find('[for="name[' + emailIndex + ']"]').remove();
                        $('#email-addresses').find('br').last().remove();
                    }
                    if (emailIndex == 1) {
                        $('a.delete').remove();
                    }
                });
            }
            emailIndex++;
            return false;
        });

        if (emails.length > 0) {
            if (companyPaying == true) {
                $('#company_pay_yes').attr('checked', 'checked');
                setupLabel();
                $('#dashboard-setup-company-tenant2').find('.ul_email').show();
                var i;
                for (i = 0; i < emails.length; ++i) {
                    if (i == 0) {
                        $('#email-addresses').find('input[class="email"]').val(emails[i]);
                        $('#email-addresses').find('input[class="name"]').val(names[i]);
                        if (paying[i] == 1) {
                            $('#email-addresses').find('label[class="label_check"]').click();
                            setupLabel();
                        }
                            
                    }
                    else {
                        $('a.add').trigger('click');
                        $('#email-addresses').find('[name="employee_email[' + (emailIndex-1) + ']"]').val(emails[i]);
                        $('#email-addresses').find('[name="name[' + (emailIndex - 1) + ']"]').val(names[i]);
                        if (paying[i] == 1) {
                            $('#email-addresses').find('[for="are_paying[' + (emailIndex - 1) + ']"]').click();
                            setupLabel();
                        }

                    }
                }
            }
            else {
                $('#company_pay_no').attr('checked', 'checked');
                setupLabel();
            }
            $('.dashboard-setup').center();
        }
        else {
            $('#company_pay_no').attr('checked', 'checked');
            setupLabel();
        }

        $('#dashboard-setup-company-tenant2').find('[name=continue]').click(function (event) {
            event.preventDefault();
            if ($('#dashboard-setup-company-tenant2').find('.label_radio input:checked').length >0) {         
                if ($("#company-emails").valid()) {
                    companyPaying = $('.label_radio input:checked').val() == 'yes';
                    if ($('.label_radio input:checked').val() == 'yes') {
                        var emailsString;
                        var namesString;
                        var payingString;
                        emails.length = 0;
                        names.length = 0;
                        paying.length = 0;
                        $('#email-addresses').find('input').filter(".email").each(function (index) {
                            emails[index] = $.trim($(this).val());
                            emailsString = (index == 0 ? $.trim($(this).val()) : emailsString + ' ' + $.trim($(this).val()));
                        });
                        $('#email-addresses').find('input').filter(".name").each(function (index) {
                            names[index] = $.trim($(this).val());
                            namesString = (index == 0 ? $.trim($(this).val()) : namesString + '$' + $.trim($(this).val()));
                        });
                        $('#email-addresses').find('label').filter(".label_check").each(function (index) {
                            paying[index] = $(this).hasClass("c_on") ? 1 : 0;
                            payingString = (index == 0 ? paying[index] : payingString + '$' + paying[index]);
                        });
                        $.ajax({
                            url: window.sendCompanyEmailsUrl,
                            type: "POST",
                            dataType: "json",
                            cache: self.cacheEnable,
                            data: {
                                'emailsString': emailsString,
                                'namesString': namesString,
                                'payingString': payingString,
                            },
                            success: function (response) {
                                if (response.success) {
                                    if (isVerify) {
                                        self.GetVerify(true);
                                    } else {
                                        self.GetPayment4(true);
                                    }
                                }
                            },
                            error: function (XMLHttpRequest, etype, exo) {
                                self.common.ShowErrorBox(etype, exo);
                                return;
                            }
                        });
                    }
                    else {
                        if (isVerify) {
                            self.GetVerify(true);
                        } else {
                            self.GetPayment4(true);
                        }
                    }
                }
            }
            return false;
        });
    };

    this.GetPayment4 = function (isTenant) {
        $.ajax({
            url: "ajax/dashboard-setup-payment4-tenant.html",
            type: "GET",
            dataType: "html",
            cache: self.cacheEnable,
            success: function (html) {
                $('#overlay').show();
                $('.pop-up').filter(':not(#login)').remove();
                $('body').append(html);
                self.SetPayment4(isTenant);
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });
    };

    this.GetVerify = function (isTenant) {
        $.ajax({
            url: (!isTenant ? "ajax/dashboard-setup-verify.html" : "ajax/dashboard-setup-verify-tenant.html"),
            type: "GET",
            dataType: "html",
            cache: self.cacheEnable,
            success: function (html) {
                $('#overlay').show();
                $('.pop-up').filter(':not(#login)').remove();
                $('body').append(html);
                (!isTenant ? self.GetVerifyEmployer() : self.GetVerifyTenant());
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });
    };

    this.GetVerifyEmployer = function () {
        $('#backstretch').remove();
        $('.dashboard-setup').center();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });

        $('#dashboard-setup').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        //profile
        if (profileData.BuildingToLotName == undefined && profileData.BuildingToLotID > 0) {
            $.ajax({
                url: window.listLotsUrl,
                type: "POST",
                async: true,
                dataType: "json",
                cache: self.cacheEnable,
                success: function (response) {
                    jQuery.each(response, function () {
                        if (this.BuildingToLotID == profileData.BuildingToLotID) {
                            profileData.BuildingToLotName = this.Lot.LotName + "/" + this.Building.BuildingName;
                            $('#parking_lot').text(profileData.BuildingToLotName);
                            return false;
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }
        else {
            $('#parking_lot').text(profileData.BuildingToLotName);
        }

        if (profileData.TenantName == undefined && profileData.TenantId > 0) {
            $.ajax({
                url: window.listTenantsUrl,
                type: "POST",
                async: true,
                dataType: "json",
                cache: self.cacheEnable,
                success: function (response) {
                    jQuery.each(response, function () {
                        if (this.CompanyID == profileData.TenantId) {
                            profileData.TenantName = this.CompanyName;
                            $('#tenant').text(profileData.TenantName);
                            return false;
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }
        else {
            $('#tenant').text(profileData.TenantName);
        }

        //parking
        $('#start_date').text(parkingData.StartDate);
        $('#end_date').text(parkingData.EndDate);
        $('#reserved').text(parkingData.ReservedSpaces + ' - $' + (parkingData.ReservedSpacesCost.replace('$', '') * parkingData.ReservedSpaces));
        $('#unreserved').text(parkingData.UnReservedSpaces + ' - $' + (parkingData.UnReservedSpacesCost.replace('$', '') * parkingData.UnReservedSpaces));

        //account
        $('#name').text(accountData.FirstName + ' ' + accountData.LastName);
        $('#email').text(accountData.EmailAddress);

        if (accountData.PhoneTypeName == undefined && accountData.PhoneTypeId > 0) {
            $.ajax({
                url: window.ListPhoneTypesUrl,
                type: "POST",
                dataType: "json",
                async: true,
                cache: self.cacheEnable,
                success: function (Response) {
                    jQuery.each(Response, function () {
                        if (this.PhoneTypeID == accountData.PhoneTypeId) {
                            accountData.PhoneTypeName = this.PhoneType1;
                            $('#phone_number').text(accountData.PhoneTypeName + ' - ' + accountData.PhoneNumber);
                            return false;
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                    return;
                }
            });
        }
        else {
            $('#phone_number').text(accountData.PhoneTypeName + ' - ' + accountData.PhoneNumber);
        }

        //vehicle
        if (vehicleData != null) {
            if (vehicleData.Make == undefined && vehicleData.MakeId > 0) {
                $.ajax({
                    url: window.listMakesUrl,
                    type: "POST",
                    dataType: "json",
                    cache: self.cacheEnable,
                    success: function (response) {
                        jQuery.each(response, function () {
                            if (this.VehicleMakeID == vehicleData.MakeId) {
                                vehicleData.Make = this.VehicleMake1;
                                $('#make').text(vehicleData.Make);
                                return false;
                            }
                        });
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            else {
                $('#make').text(vehicleData.Make);
            }

            if (vehicleData.Model == undefined && vehicleData.ModelId > 0) {
                $.ajax({
                    url: window.listModelsUrl,
                    type: "POST",
                    dataType: "json",
                    async: true,
                    cache: self.cacheEnable,
                    data: {
                        "makeId": vehicleData.MakeId,
                    },
                    success: function (response) {
                        jQuery.each(response, function () {
                            if (this.VehicleModelID == vehicleData.ModelId) {
                                vehicleData.Model = this.VehicleModel1;
                                $('#model').text(vehicleData.Model);
                                return false;
                            }
                        });
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            else {
                $('#model').text(vehicleData.Model);
            }

            $('#year').text(vehicleData.Year);
            $('#license').text(vehicleData.LicenseNumber);
            $('#color').text(vehicleData.Color);
            $('#permit').text(vehicleData.PermitNumber);
        }

        //payment
        if (onlineCheck != null) {
            $('#online_check').show();
            $('#name_account').text(onlineCheck.AccountName);
            $('#bankname').text(onlineCheck.BankName);
            var CheckingTypeName = '';
            switch (onlineCheck.CheckingTypeID) {
                case '1':
                    CheckingTypeName = "Personal";
                    break;
                case '2':
                    CheckingTypeName = "Business";
                    break;
                case '3':
                    CheckingTypeName = "Savings";
                    break;
            }
            $('#checking_type').text(CheckingTypeName + ' - ' + onlineCheck.AccountNumber1);
            $('#routing_number').text(onlineCheck.RoutingNumber);
        }
        else {
            $('#credit_card').show();
            if (creditCard != null) {
                $('#ch_fname').text(creditCard.CHFirstName);
                $('#ch_lname').text(creditCard.CHLastName);
                $('#card_number').text('********' + creditCard.CardNumber.substring((creditCard.CardNumber.length - 4)));
                $('#address').text(creditCard.Address1 + ' ' + creditCard.Address2);

                if (creditCard.StateName == undefined && creditCard.StateId > 0) {
                    $.ajax({
                        url: window.listStatesUrl,
                        type: "POST",
                        dataType: "json",
                        cache: self.cacheEnable,
                        async: true,
                        success: function (response) {
                            jQuery.each(response, function () {
                                if (this.StateID == creditCardModel.StateId) {
                                    creditCardModel.StateName = this.StateCode;
                                    $('#city_state_zip').text(creditCard.City + ', ' + creditCard.StateName + ' ' + creditCard.ZipCode);
                                    return false;
                                }
                            });
                        },
                        error: function (XMLHttpRequest, etype, exo) {
                            self.common.ShowErrorBox(etype, exo);
                        }
                    });
                }
                else {
                    $('#city_state_zip').text(creditCard.City + ', ' + creditCard.StateName + ' ' + creditCard.ZipCode);
                }
            }
        }

        $('#edit_profile').click(function (event) {
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
                    self.SetProfile();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('#edit_parking').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-parking.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetParking(true);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('#edit_account').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-account.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetAccount(true);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('#edit_vehicle').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-vehicle.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetVehicle(false,true);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('.edit_payment').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-payment4.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetPayment4(false);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('#dashboard-setup').find('[name=continue]').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-congratulations.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.Congratulations();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });
    };

    this.GetVerifyTenant = function () {
        $('#backstretch').remove();
        $('.dashboard-setup').center();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });

        //profile
        if (profileData.BuildingToLotName == undefined && profileData.BuildingToLotID > 0) {
            $.ajax({
                url: window.listLotsUrl,
                type: "POST",
                async: true,
                dataType: "json",
                cache: self.cacheEnable,
                success: function (response) {
                    jQuery.each(response, function () {
                        if (this.BuildingToLotID == profileData.BuildingToLotID) {
                            profileData.BuildingToLotName = this.Lot.LotName + "/" + this.Building.BuildingName;
                            $('#parking_lot').text(profileData.BuildingToLotName);
                            return false;
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }
        else {
            $('#parking_lot').text(profileData.BuildingToLotName);
        }

        //company
        $('#company_name').text(companyInfo.CompanyName);
        $('#suite').text(companyInfo.Suite);
        $('#ismaling').text(companyInfo.asBuilding ? 'yes' : 'no');
        $('#company_address').text(companyInfo.Address1 + ' ' + companyInfo.Address2);
        $('#ismanager').text(companyPerson.isManager ? 'yes' : 'no');

        if (companyInfo.StateName == undefined && companyInfo.StateId > 0) {
            $.ajax({
                url: window.listStatesUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                async: true,
                success: function (response) {
                    jQuery.each(response, function () {
                        if (this.StateID == companyInfo.StateId) {
                            companyInfo.StateName = this.StateCode;
                            $('#company_city_state_zip').text(companyInfo.City + ', ' + companyInfo.StateName + ' ' + companyInfo.ZipCode);
                            return false;
                        }
                    });
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });
        }
        else {
            $('#company_city_state_zip').text(companyInfo.City + ', ' + companyInfo.StateName + ' ' + companyInfo.ZipCode);
        }

        //vehicle
        if (vehicleData != null ) {
            if (vehicleData.Make == undefined && vehicleData.MakeId > 0) {
                $.ajax({
                    url: window.listMakesUrl,
                    type: "POST",
                    dataType: "json",
                    cache: self.cacheEnable,
                    success: function (response) {
                        jQuery.each(response, function () {
                            if (this.VehicleMakeID == vehicleData.MakeId) {
                                vehicleData.Make = this.VehicleMake1;
                                $('#make').text(vehicleData.Make);
                                return false;
                            }
                        });
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            else {
                $('#make').text(vehicleData.Make);
            }

            if (vehicleData == null && vehicleData.Model == undefined && vehicleData.ModelId > 0) {
                $.ajax({
                    url: window.listModelsUrl,
                    type: "POST",
                    dataType: "json",
                    async: true,
                    cache: self.cacheEnable,
                    data: {
                        "makeId": vehicleData.MakeId,
                    },
                    success: function (response) {
                        jQuery.each(response, function () {
                            if (this.VehicleModelID == vehicleData.ModelId) {
                                vehicleData.Model = this.VehicleModel1;
                                $('#model').text(vehicleData.Model);
                                return false;
                            }
                        });
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.common.ShowErrorBox(etype, exo);
                    }
                });
            }
            else {
                $('#model').text(vehicleData.Model);
            }

            $('#year').text(vehicleData.Year);
            $('#license').text(vehicleData.LicenseNumber);
            $('#color').text(vehicleData.Color);
            $('#permit').text(vehicleData.PermitNumber);
        }
        $('#personally').text(personally ? 'yes' : 'no');

        //account
        $('#company_paying').text(companyPaying ? 'yes' : 'no');
        var email_string = '';
        if (emails.length > 0) {
            for (i = 0; i < emails.length; ++i) {
                if (i == 0) {
                    email_string = emails[0];
                }
                else {
                    email_string += ', ' + emails[i];
                }
            }
        }
        $('#email_string').text(email_string);

        //payment
        if (onlineCheck != null) {
            $('#online_check').show();
            $('#name_account').text(onlineCheck.AccountName);
            $('#bankname').text(onlineCheck.BankName);
            var CheckingTypeName = '';
            switch (onlineCheck.CheckingTypeID) {
                case '1':
                    CheckingTypeName = "Personal";
                    break;
                case '2':
                    CheckingTypeName = "Business";
                    break;
                case '3':
                    CheckingTypeName = "Savings";
                    break;
            }
            $('#checking_type').text(CheckingTypeName + ' - ' + onlineCheck.AccountNumber1);
            $('#routing_number').text(onlineCheck.RoutingNumber);
        }
        else {
            $('#credit_card').show();
            if (creditCard != null) {
                $('#ch_fname').text(creditCard.CHFirstName);
                $('#ch_lname').text(creditCard.CHLastName);
                $('#card_number').text('********' + creditCard.CardNumber.substring((creditCard.CardNumber.length - 4)));
                $('#address').text(creditCard.Address1 + ' ' + creditCard.Address2);

                if (creditCard.StateName == undefined && creditCard.StateId > 0) {
                    $.ajax({
                        url: window.listStatesUrl,
                        type: "POST",
                        dataType: "json",
                        cache: self.cacheEnable,
                        async: true,
                        success: function (response) {
                            jQuery.each(response, function () {
                                if (this.StateID == creditCardModel.StateId) {
                                    creditCardModel.StateName = this.StateCode;
                                    $('#city_state_zip').text(creditCard.City + ', ' + creditCard.StateName + ' ' + creditCard.ZipCode);
                                    return false;
                                }
                            });
                        },
                        error: function (XMLHttpRequest, etype, exo) {
                            self.common.ShowErrorBox(etype, exo);
                        }
                    });
                }
                else {
                    $('#city_state_zip').text(creditCard.City + ', ' + creditCard.StateName + ' ' + creditCard.ZipCode);
                }
            }
        }

        $('#edit_profile').click(function (event) {
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
                    self.SetProfile();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('#edit_company').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-company2-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetCompany(true);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('#edit_vehicle').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-parking-vehicle2-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetVehicle(true,true);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('#edit_account').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-payment2-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetPayment1(true);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('.edit_payment').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-payment4-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.SetPayment4(true);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });

        $('#dashboard-setup').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup').find('[name=continue]').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/dashboard-setup-congratulations-tenant.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').show();
                    $('.pop-up').filter(':not(#login)').remove();
                    $('body').append(html);
                    self.Congratulations();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.common.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });
    };

    this.Congratulations = function () {
        $('#backstretch').remove();
        $('.dashboard-setup').center();
        $.backstretch("/images/complete-bg.jpg", { speed: 'slow' });

        $('#dashboard-setup').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('.pop-up').filter(':not(#login)').remove();
            $('#login').show();

            $('#backstretch').remove();
            $.backstretch("/images/home-bg.jpg", { speed: 'slow' });

            return false;
        });

        $('#dashboard-setup').find('[name=finish]').click(function (event) {
            event.preventDefault();
            self.GetDashboard();
            return false;
        });
    };

    this.GetDashboard = function () {
        $.ajax({
            url: window.setStatus,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            data: {
                "statusId": 1,
            },
            success: function (html) {
                window.location.href = window.dashboardUrl;
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });
    };

    function setupLabel() {
        if ($('.label_check input').length) {
            $('.label_check').each(function () {
                $(this).removeClass('c_on');
            });
            $('.label_check input:checked').each(function () {
                $(this).parent('label').addClass('c_on');
            });
        };
        if ($('.label_radio input').length) {
            $('.label_radio').each(function () {
                $(this).removeClass('r_on');
            });
            $('.label_radio input:checked').each(function () {
                $(this).parent('label').addClass('r_on');
            });
        };
    };

    function renderDate(pDate) {
        if (pDate != '') {
            var javascriptDate = new Date(parseInt(pDate.slice(6, -2)));
            javascriptDate = javascriptDate.getMonth() + "/" + javascriptDate.getDate() + "/" + javascriptDate.getFullYear();
            return javascriptDate;
        }
        return '';
    };

    function resetCachedInfo() {
        $.ajax({
            url: "./Account/SessionClear",
            type: "POST",
            dataType: "json",
            success: function (response) {
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.common.ShowErrorBox(etype, exo);
            }
        });

        profileData = null;
        parkingData = null;
        accountData = null;
        vehicleData = null;
        paymentType = '';
        onlineCheck = null;
        creditCard = null;

        companyInfo = null;
        companyPerson = null;
        personally = false;
        companyPaying = false;
        emails = [];
    };

    //jQuery.validator.addMethod("notEqual", function (value, element, param) {
    //    return this.optional(element) || value !== "Name";
    //}, "*");

    this.owner = new RegistrationBuildingOwner(self.SetProfile);
}