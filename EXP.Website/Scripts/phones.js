function Phones(commonFunctionality) {
    var self = this;
    this.common = commonFunctionality;
    this.cacheEnable = false;
    this.mass_length;
    this.locCommon = new Common();   
    this.Start = function () {
        self.TakeAllPhonesList();
        self.common.AttachPopup("editPhoneLink", "/ajax/editphone.html", self.EditPhone);
        self.common.AttachPopup("removePhoneLink", "/ajax/removephone.html", self.RemovePhone);
        self.common.AttachPopup("newPhoneLink", "/ajax/newphone.html", self.NewPhone);     
    };

    this.TakeAllPhonesList = function () {
        
        var phones_from_server;
        
        $.ajax({
            url: window.getPhonesUrl,
            type: "POST",
            async: false,
            dataType: "json",
            cache: self.cacheEnable,          
            success: function (responseO) {
                if (responseO.success) {
                    phones_from_server = responseO.phoneobj.phones;
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
        
        var script = document.createElement('script');
        script.innerHTML += 'var phones =' + phones_from_server;
        document.getElementsByTagName('head')[0].appendChild(script);

        self.mass_length = window.phones.length;

    }

    var listPhoneTypes;

    this.NewPhone = function () {

        $.validator.addMethod("phoneformat",
                              function (value, element) {
                                  return value.match(/\(\d\d\d\)\d\d\d\-\d\d\d\d$/);
                              },
                             "Should be the format (000)000-0000"
                             );

        $("input#phone_number").mask("(999)999-9999");

        $("#newPhoneForm").validate({
            rules: {
                phone_number: {
                    required: true,
                    phoneformat: true,
                },
                extension: {
                    digits: true,
                    maxlength: 8,
                },
            },
        });

        if (listPhoneTypes=='' || listPhoneTypes==undefined) {
            self.GetListPhoneTypes();
        }

        jQuery.each(listPhoneTypes,function() {
            $('[name=phone_type]').append("<option value='" + this.PhoneTypeID + "'>" + this.PhoneType1 + "</option>");
        });

        $('.pop-up').find('[name=save]').click(function (event) {
            event.preventDefault();

            $("#newPhoneForm").validate();
            if (!$("#newPhoneForm").valid()) {
                return false;
            }

            var newPhoneModel = new Object();
            newPhoneModel.PhoneTypeID = $('.pop-up').find('[name=phone_type]').val();
            newPhoneModel.AreaCode = $('.pop-up').find('[name=extension]').val();
            newPhoneModel.PhoneNumber = $('.pop-up').find('[name=phone_number]').val();

            $.ajax({
                url: window.newPhoneUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: newPhoneModel,
                success: function (newPhoneResponse) {
                    $('#overlay').hide();
                    $('.pop-up').remove();
                    self.DisplayPhone(newPhoneResponse.newPhoneId);
                    $('#spanManagePhones').show();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
            return false;
        });
    };

    this.EditPhone = function () {
        $.validator.addMethod("phoneformat",
                              function (value, element) {
                                  return value.match(/\(\d\d\d\)\d\d\d\-\d\d\d\d$/);
                              },
                             "Should be the format (000)000-0000"
                             );

        $("input#phone_number").mask("(999)999-9999");


        $("#editPhoneForm").validate({
            rules: {
                phone_number: {
                    required: true,
                    phoneformat:true
                },
                extension: {
                    digits: true,
                    maxlength: 8,
                },
            },
        });

        if (listPhoneTypes=='' || listPhoneTypes==undefined) {
            self.GetListPhoneTypes();
        }

        jQuery.each(listPhoneTypes,function() {
            $('[name=phone_type]').append("<option value='" + this.PhoneTypeID + "'>" + this.PhoneType1 + "</option>");
        });

        $.ajax({
            url: window.listPhonesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                var dropDown = $('#phone').find('[name=phones]');
                dropDown.empty();
                if (response.length > 0) {
                    $.each(response, function(index, value) {
                        $(dropDown).append('<option value="' + value.PhoneID + '">' + value.PhoneNumber + ' ('+ value.PhoneType.PhoneType1 + ')</option>');
                    });
                     $('.pop-up').find('[name=phones]').change();
                }
                else {
                    $('.pop-up').find('[name=save]').attr("disabled", "disabled");
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });

        $('.pop-up').find('[name=phones]').change(function() {
            $.ajax({
                url: window.getPhoneUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: {
                    "phoneId": $('.pop-up').find('[name=phones]').val(),
                },

                success: function (getPhoneResponse) {
                    if (getPhoneResponse.success) {
                        $('.pop-up').find('[name=phone_type]').val(getPhoneResponse.phone.PhoneTypeID);
                        $('.pop-up').find('[name=phone_number]').val(getPhoneResponse.phone.PhoneNumber);
                        $('.pop-up').find('[name=extension]').val(getPhoneResponse.phone.AreaCode);   
                    }
                    else {                        
                        self.locCommon.ShowErrorBox('Error', getPhoneResponse.error);
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
        });

        $('.pop-up').find('[name=save]').click(function (event) {
            event.preventDefault();

            $("#editPhoneForm").validate();
            if (!$("#editPhoneForm").valid()) {
                return false;
            }

            var editPhoneModel = new Object();
            editPhoneModel.PhoneID = $('.pop-up').find('[name=phones]').val();
            editPhoneModel.PhoneTypeID = $('.pop-up').find('[name=phone_type]').val();
            editPhoneModel.AreaCode = $('.pop-up').find('[name=extension]').val();
            editPhoneModel.PhoneNumber = $('.pop-up').find('[name=phone_number]').val();

            $.ajax({
                url: window.editPhoneUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: editPhoneModel,
                success: function (newPhoneResponse) {
                    self.ChangePhone(newPhoneResponse.phoneId);
                    $('#overlay').hide();
                    $('.pop-up').remove();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                    return;
                }
            });
            return;
        });
    };
    
    this.RemovePhone = function () {
        $.ajax({
            url: window.listPhonesUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            success: function (response) {
                var dropDown = $('#phone').find('[name=phones]');
                dropDown.empty();
                if (response.length > 0) {
                    $.each(response, function(index, value) {
                        $(dropDown).append('<option value="' + value.PhoneID + '">' + value.PhoneNumber + ' ('+ value.PhoneType.PhoneType1 + ')</option>');
                    });
                }
                else {
                    $('.pop-up').find('[name=remove]').css("visibility", "hidden");
                }
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
            }
        });

        $('.pop-up').find('[name=remove]').click(function (event) {
            event.preventDefault();

            var removePhoneModel = new Object();
            removePhoneModel.PhoneID = $('.pop-up').find('[name=phones]').val();

            $.ajax({
                url: window.removePhoneUrl,
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: removePhoneModel,
                success: function (removePhoneResponse) {
                    self.RemovePhoneFromList(removePhoneModel.PhoneID);
                    $('#overlay').hide();
                    $('.pop-up').remove();
                    if (removePhoneResponse.phonesCount == 0) {
                        $('#spanManagePhones').hide();
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

    this.GetListPhoneTypes = function () {
        $.ajax({
            url: window.ListPhoneTypesUrl,
            type: "POST",
            dataType: "json",
            async: false,
            cache: self.cacheEnable,
            success: function (Response) {
                listPhoneTypes = Response;         
            },
            error: function (XMLHttpRequest, etype, exo) {
                self.locCommon.ShowErrorBox(etype, exo);
                return;
            }
        });
        return;
    };

    this.DisplayPhone = function (phoneID) { 
        $.ajax({
            url: window.getPhoneUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            data: {
                "phoneId": phoneID,
            },
            success: function (response) {
                if (response.success) {
                    $('#phones').children().last().remove();
                    if ($('#phones').children().last().is('br')) {
                        $('#phones').children().last().remove();
                    }  
                    $('#phones').prepend('<span id="phone' + phoneID + '">' + response.phone.PhoneNumber + ' (' + response.phone.PhoneType.PhoneType1 + ')</span><br/>');
                    window.phones.push({ "id": phoneID +"", "number": response.phone.PhoneNumber, "type1": response.phone.PhoneType.PhoneType1 });
                    self.mass_length++;
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

    this.ChangePhone = function (phoneId) {
        $.ajax({
            url: window.getPhoneUrl,
            type: "POST",
            dataType: "json",
            cache: self.cacheEnable,
            data: {
                "phoneId": phoneId,
            },
            success: function (response) {
                if (response.success) {
                    var replaceRow = $('#phone' + response.phone.PhoneID).closest("span");
                    replaceRow.text(response.phone.PhoneNumber + ' (' + response.phone.PhoneType.PhoneType1 + ')');

                    var i = 0;
                    while (i < self.mass_length) {
                        if (window.phones[i] != undefined) {
                            if (window.phones[i].id == phoneId) {
                                window.phones[i].number = response.phone.PhoneNumber;
                                window.phones[i].type1 = response.phone.PhoneType.PhoneType1;
                            }
                        }
                        i++;
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
    };

    this.RemovePhoneFromList = function (phoneID) {
        //$('#phones').find('#phone' + phoneID).remove();
        
        var i = 0;
        while (i < self.mass_length) {
            if (window.phones[i] != undefined) {
                if (window.phones[i].id == phoneID) {
                    delete window.phones[i];
                    //self.mass_length--;
                }
            }
            i++;
        }
        if (self.mass_length > 0) {
            $('#phones').empty();
            var i = 0;
            var j = self.mass_length - 1;
            while (j >= 0 && i < 2) {
                if (window.phones[j] != undefined) {
                    i++;
                    $('#phones').append('<span id="phone' + window.phones[j].id + '">' + window.phones[j].number + '(' + window.phones[j].type1 + ')</span>');
                    if (i == 1) {
                        $('#phones').append('<br/>');
                    }
                }
                j--;
            }
        }
        else {
            $('#phones').empty();
        }                
    };
    self.Start();
}