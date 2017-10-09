function Common() {
    var self = this;
    this.cacheEnable = false;

    //Custom Checkboxes and radio buttons
    this.OverlayShow = function () {
        var hOverlay = $("#overlay").height();
        var hWrapper = $("#wrapper").height();
        var height = (hOverlay > hWrapper ? hOverlay : hWrapper);
        $('#overlay').height(height);
        $('#overlay').show();
    };


    this.setupLabel = function () {
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

    this.AttachPopup = function (link, popupHtmlFile, afterOpenFunctionality) {
        $('#' + link).click(function (event) {
            event.preventDefault();

            $.ajax({
                url: popupHtmlFile,
                type: "GET",
                dataType: "html",
                cache: self.cacheEnable,
                success: function (html) {
                    self.OverlayShow();
                    $('.pop-up').remove();
                    $('body').append(html);
                    $('.pop-up').find('.close,[name=cancel]').click(function (event1) {
                        event1.preventDefault();

                        $('#overlay').hide();
                        $('.pop-up').remove();

                        return false;
                    });
                    afterOpenFunctionality();
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.ShowErrorBox(etype, exo);
                }
            });
            return false;
        });
    };

    this.AttachPopupforEditBuildingAddress = function (popupHtmlFile, afterOpenFunctionality) {
        var buildingId;
        $('input[name="add_building"]').click(function (event) {
            buildingId = 0;

            $.ajax({
                url: popupHtmlFile,
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
                    afterOpenFunctionality(buildingId);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    self.ShowErrorBox(etype, exo);
                }
            });
        });
        // For Updating building here

        $('#buildings a[href="/editemail.html"]').each(function () {
            $(this).click(function (event) {
                event.preventDefault();
                buildingId = $(this).attr('data-id');
                address2 = $(this).parent('p').find("[data-id='address2']").text();

                $.ajax({
                    url: popupHtmlFile,
                    type: "GET",
                    dataType: "html",
                    cache: self.cacheEnable,
                    success: function (html) {
                        self.OverlayShow();
                        $('.pop-up').remove();
                        $('body').append(html);
                        $("[name='buildingId']").attr("value", buildingId);
                        $("[name='address2']").attr("value", address2);
                        $('.pop-up').find('.close,[name=cancel]').click(function (event1) {
                            event1.preventDefault();

                            $('#overlay').hide();
                            $('.pop-up').remove();

                            return false;
                        });
                        afterOpenFunctionality(buildingId);
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        self.ShowErrorBox(etype, exo);
                    }
                });
                return false;
            });
        });

        // for Deleting building here

        $('#buildings a[href="#"]').each(function () {
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
                            self.ShowErrorMessage(response.error);
                        }
                    },
                    error: function (XMLHttpRequest, etype, exo) {
                        alert(etype);
                        return;
                    }
                });
            });
        });

    };

    this.ShowErrorMessage = function (error) {
        if (jQuery.type(error) === "string") {            
            self.ShowErrorBox('Error', error);
        } else {
            var errorString = '';
            jQuery.each(error, function () {
                errorString += this + '\n';
            });            
            self.ShowErrorBox('Error', errorString);
        }
    };

    this.ShowErrorBox = function(MessageType, ErrorMessage) {
        $.ajax({
            url: "/ajax/profileerror.html",
            type: "GET",
            dataType: "html",
            success: function(html) {
                $('#overlay').css('z-index', '102');
                $('#overlay').show();
                $('body').append(html);

                $('#profile-error').center();
                $('#profile-error').find('h3').text(MessageType);

                $('#profile-error').find('p').text(ErrorMessage);

                $('#profile-error').find('p').css('max-width', '200px');
                $('#profile-error').find('p').css('word-wrap', 'break-word');
            },
            error: function(XMLHttpRequest, etype, exo) {
                self.ShowErrorBox(etype, exo);
            }
        });
    };
}