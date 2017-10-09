function Payments(commonFunctionality, alertsFunctionality) {
    var self = this;
    this.common = commonFunctionality;
    this.cacheEnable = false;
    this.alerts = alertsFunctionality;
    this.locCommon = new Common();
    this.my_addition = My_addition();

    this.Start = function () {
        self.common.AttachPopup("editCreditCardLink", "/ajax/editcreditcard.html", self.EditPayment);
        self.common.AttachPopup("editCreditCardLink1", "/ajax/editcreditcard.html", self.EditPayment);
    };

    this.EditPayment = function () {

        $('.label_radio').click(function () {
            setupLabel();
        });
        $('#manage_payment_method').hide();
        $('#credit_card').hide();
        $('#online_check').hide();

        var payment_method;
        var oldAddressId;
        var init = true;
        var homeAddress = [6];

        $("input[name=card_number]").mask("9999 9999 9999 9999");
        $("input[name=routing_number]").mask("999999999");

        $('#credit_card').validate({
            rules: {
                card_number: {
                    cardformat: true,
                },
            },
            messages: {
                card_number: {},
            }
        });
        
        $("#online_check").validate({
            rules: {
                routing_number: {
                    routignumber: true,
                },
            },
            messages: {
                routing_number: {},
            }
        });

        $('[name=payment_method]').change(function () {
            $('#credit_card').hide();
            $('#online_check').hide();
            if ($('[name=payment_method]').val() != '') {
                block = '#' + $('[name=payment_method]').val();
                $(block).show(0, function () { });
                payment_method = $('[name=payment_method]').val();
                if (payment_method == 'credit_card' && init) {

                    init = false;
                    setupFieldsCC();
                }
            }
        });

        $.ajax({
            url: getPaymentUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                if (response.PaymentID == 0) {
                    $('#manage_payment_method').show();
                    $('[name=payment_method]').change();
                }
                else {
                    $('.pop-up').find('[name=paymentId]').val(response.PaymentID);
                    if (response.isCreditCard) {
                        $('#credit_card').show();
                        setupFieldsCC();
                        payment_method = 'credit_card';
                        $('.pop-up').find('[name=creditCardId]').val(response.CreditCard.CreditCardID);
                        $('.pop-up').find('[name=addressId]').val(response.CreditCard.AddressID);
                        oldAddressId = response.CreditCard.AddressID;
                        $('.pop-up').find('[name=ch_first_name]').val(response.CreditCard.CHFirstName);
                        $('.pop-up').find('[name=ch_last_name]').val(response.CreditCard.CHLastName);
                        $('.pop-up').find('[name=card_number]').val(getCardNumber(response.CreditCard.CardNumber));
                        $('.pop-up').find('[name=month]').val(response.CreditCard.ExpDateMount);
                        $('.pop-up').find('[name=year]').val(response.CreditCard.ExpDateYear);
                        $('.pop-up').find('[name=cvv]').val(response.CreditCard.CVV);
                        $('.pop-up').find('[name=auto_pay]').attr("checked", response.CreditCard.AutoPay).parent().css('background-image', (response.CreditCard.AutoPay == true ? 'url("../images/check-on.png")' : 'url("../images/check-off.png")'));
                        $('.pop-up').find('[name=billing_address]').attr("checked", response.CreditCard.isHome).parent().css('background-image', (response.CreditCard.isHome ? 'url("../images/check-on.png")' : 'url("../images/check-off.png")'));
                        $('.pop-up').find('[name=address1]').val(response.CreditCard.Address1);
                        $('.pop-up').find('[name=address2]').val(response.CreditCard.Address2);
                        $('.pop-up').find('[name=city]').val(response.CreditCard.City);
                        $('.pop-up').find('[name=state]').val(response.CreditCard.StateID);
                        $('.pop-up').find('[name=zip]').val(response.CreditCard.ZipCode);
                    }
                    else {
                        $('#online_check').show();
                        payment_method = 'online_check';

                        $('.pop-up').find('[name=onlineCheckId]').val(response.OnlineCheck.OnlineCheckId);
                        $('.pop-up').find('[name=account_name]').val(response.OnlineCheck.NameOnAccount);
                        $('input:radio[name="checking_type"]').filter('[value="' + response.OnlineCheck.OnlineCheckingTypeID + '"]').attr('checked', true).parent().addClass('r_on');
                        $('.pop-up').find('[name=account_number]').val(response.OnlineCheck.CheckingAccountNumber);
                        $('.pop-up').find('[name=account_number2]').val(response.OnlineCheck.CheckingAccountNumber);
                        $('.pop-up').find('[name=bank_name]').val(response.OnlineCheck.BankName);
                        $('.pop-up').find('[name=routing_number]').val(response.OnlineCheck.RoutingNumber);
                    }
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
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

                $('[name=billing_address]').attr("checked", "checked");
                $(this).css('background-image', 'url("../images/check-on.png")');

                if (homeAddress.length == 1) {
                    $.ajax({
                        url: window.getPersonallyAddressUrl,
                        type: "POST",
                        dataType: "json",
                        async: false,
                        cache: self.cacheEnable,
                        success: function (getAddressResponse) {
                            if (getAddressResponse.success) {
                                homeAddress[0] = getAddressResponse.address.AddressID;
                                homeAddress[1] = getAddressResponse.address.Address1;
                                homeAddress[2] = getAddressResponse.address.Address2;
                                homeAddress[3] = getAddressResponse.address.City;
                                homeAddress[4] = getAddressResponse.address.StateID;
                                homeAddress[5] = getAddressResponse.address.ZipCode;
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
                }
                $('[name=addressId]').val(homeAddress[0]);
                $('[name=address1]').val(homeAddress[1]);
                $('[name=address2]').val(homeAddress[2]);
                $('[name=city]').val(homeAddress[3]);
                $('[name=state]').val(homeAddress[4]);
                $('[name=zip]').val(homeAddress[5]);
          //      $('[name=state]').bindOptions();
            }
            else {
                $(this).css('background-image', 'url("../images/check-off.png")');
                $('[name=billing_address]').removeAttr("checked");
                $('[name=addressId]').val('');
                $('[name=address1]').val('');
                $('[name=address2]').val('');
                $('[name=city]').val('');
                $('[name=state]').val('');
                $('[name=zip]').val('');
           //     $('[name=state]').val('').bindOptions();
            }
        });

        $('#credit_card_billing').find('[name=save]').click(function (event) {
            event.preventDefault();

            if (payment_method == "online_check") {
                if ($('#online_check').valid()) {
                    var onlineCheckModel = new Object();
                    onlineCheckModel.onlineCheckId = $('.pop-up').find('[name=onlineCheckId]').val();
                    onlineCheckModel.NameOnAccount = $('.pop-up').find('[name=account_name]').val();
                    onlineCheckModel.OnlineCheckingTypeID = $('input:radio[name="checking_type"]').filter(":checked").val();
                    onlineCheckModel.CheckingAccountNumber = $('.pop-up').find('[name=account_number]').val();
                    onlineCheckModel.BankName = $('.pop-up').find('[name=bank_name]').val();
                    onlineCheckModel.RoutingNumber = $('.pop-up').find('[name=routing_number]').val();
                    saveInfo(payment_method, onlineCheckModel);
                }
            }
            else {
                if (payment_method == "credit_card" && $('#credit_card').valid()) {
                    var creditCardModel = new Object();
                    creditCardModel.CreditCardId = $('.pop-up').find('[name=creditCardId]').val();
                    creditCardModel.AddressId = $('.pop-up').find('[name=addressId]').val();
                    creditCardModel.OldAddressId = oldAddressId;
                    creditCardModel.CHFirstName = $('.pop-up').find('[name=ch_first_name]').val();
                    creditCardModel.CHLastName = $('.pop-up').find('[name=ch_last_name]').val();
                    creditCardModel.CardNumber = $('.pop-up').find('[name=card_number]').val();
                    creditCardModel.ExpDateMount = $('.pop-up').find('[name=month]').val();
                    creditCardModel.ExpDateYear = $('.pop-up').find('[name=year]').val();
                    creditCardModel.CVV = $('.pop-up').find('[name=cvv]').val();
                    creditCardModel.AutoPay = ($('.pop-up').find('[name=auto_pay]').attr("checked") == "checked" ? true : false);
                    creditCardModel.isHome = ($('.pop-up').find('[name=billing_address]').attr("checked") == "checked" ? true : false);
                    creditCardModel.Address1 = $('.pop-up').find('[name=address1]').val();
                    creditCardModel.Address2 = $('.pop-up').find('[name=address2]').val();
                    creditCardModel.City = $('.pop-up').find('[name=city]').val();
                    creditCardModel.StateId = $('.pop-up').find('[name=state]').val();
                    creditCardModel.ZipCode = $('.pop-up').find('[name=zip]').val();
                    saveInfo(payment_method, creditCardModel);
                }
            }
            
            return false;
        });

        return;
    };

    function getCardNumber(value) {
        var number = '';
        for (var i = 0; i < value.length; i += 4) {
            number += value.substring(i, i + 4);
            if (i != 13) {
                number += " ";
            }

        }
        return number;
    };

    function saveInfo(payment_method, model) {
        if ($('[name=paymentId]').val() == '' && $('.pop-up').find('[name=onlineCheckId]').val() == '') {
            $.ajax({
                url: payment_method == "credit_card" ? newPaymentCCUrl : newPaymentOCUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: model,
                success: function (response) {
                    $('#overlay').hide();
                    $('.pop-up').remove();
                    if (response.alerts != null) {
                        jQuery.each(response.alerts, function () {
                            if (this.isActive == false) {
                                self.alerts.RemoveAlert(this.AlertID);
                            }
                            else {
                                self.alerts.CreateAlert(this);
                            }
                        });
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
        }
        else {
            $.ajax({
                url: payment_method == "credit_card" ? editCreditCardUrl : editOnlineCheckUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: model,
                success: function (response) {
                    $('#overlay').hide();
                    $('.pop-up').remove();
                    if (response.alerts != null) {
                        jQuery.each(response.alerts, function () {
                            if (this.isActive == false) {
                                self.alerts.RemoveAlert(this.AlertID);
                            }
                            else {
                                self.alerts.CreateAlert(this);
                            }
                        });
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
        }
        return false;
    };

    function setupFieldsCC() {
        var d = new Date();

        for (var i = 0; i < 10; i++) {
            $('[name=year]').append("<option value='" + (d.getFullYear() + i) + "'>" + (d.getFullYear() + i) + "</option>")
        }

        $.ajax({
            url: window.listStatesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            async: false,
            success: function (response) {
                jQuery.each(response, function () {
                    $('[name=state]').append("<option value='" + this.StateID + "'>" + this.StateName + "</option>");
                });
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });
    };

    function setupLabel() {
        if ($('.label_radio input').length) {
            $('.label_radio').each(function () {
                $(this).removeClass('r_on');
            });
            $('.label_radio input:checked').each(function () {
                $(this).parent('label').addClass('r_on');
            });
        };
    };

    self.Start();
}