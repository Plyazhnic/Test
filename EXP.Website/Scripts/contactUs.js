$(document).ready(function () {
    var contactUs = new ContactUs();
    contactUs.Start();
});
function ContactUs(commonFunctionality) {
    var self = this;
    this.common = commonFunctionality;
    this.cacheEnable = false;
    this.locCommon = new Common();

    this.Start = function () {
        self.DisplayContactUs();
    };

    this.DisplayContactUs = function () {
        $('.contact_us').click(function (event) {
            event.preventDefault();

            $.ajax({
                url: "ajax/contactUs.html",
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    $('#overlay').css('z-index', '102');
                    $('#overlay').show();
                    $('body').append(html);
                    self.CreateContact();
                    
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.locCommon.ShowErrorBox(etype, exo);
                }
            });

            return false;
        });
    };

    this.CreateContact = function () {
        $("#contact_us-form").validate({

            rules: {
                name: {
                    required: true
                },
                email: {
                    required: true,
                    email: true
                },
                message: {
                    required: true,
                    maxlength: 500
                },
            },
            messages: {
                name: { required: "Enter your name" },
                email: { required: "Enter email", email: "Incorrect email format" },
                message: { required: "Enter message" },
            }
        });

        $('#contact-us').find('.close').click(function (event) {
            event.preventDefault();

            $('#overlay').hide();
            $('#overlay').css('z-index', '100');
            $('.pop-up').filter('#contact-us').remove();
            return false;
        });

        $('#contact-us').find('[name=submit]').click(function (event) {
            event.preventDefault();
            if ($("#contact_us-form").valid()) {
                var ContactUsModel = new Object();
                ContactUsModel.Name = $('#contact_us-form').find('input[name="name"]').val();
                ContactUsModel.Email = $('#contact_us-form').find('input[name="email"]').val();
                ContactUsModel.Message = $('#contact_us-form').find('#message').val();
                $.ajax({
                    url: "./Home/ContactUs",
                    type: "POST",
                    cache: self.cacheEnable,
                    dataType: "json",
                    data: ContactUsModel,              
                    success: function (response) {
                        if (response.success) {
                            $.ajax({
                                url: "ajax/contactussent.html",
                                type: "GET",
                                dataType: "html",
                                success: function (html) {
                                    $('#contact-us').remove();
                                    $('#overlay').show();
                                    $('body').append(html);
                                },
                                error: function (XMLHttpRequest, etype, exo) {
                                    self.locCommon.ShowErrorBox(etype, exo);
                                }
                            });
                        } else {                            
                            self.locCommon.ShowErrorBox('Error', response.error);
                        }
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.locCommon.ShowErrorBox(etype, exo);
                    }
                });
            }
        });
    };
}