function My_addition() {
    var self = this;
    this.cacheEnable = false;

    $.validator.addMethod("cardformat",
            function (value, element) {
                return value.match(/(3|4|5)(\d{3}) (\d{4}) (\d{4}) (\d{4})/);
            },
            "Should be the format 0000 0000 0000 0000 and start with 3, 4 or 5"
    );
    
    $.validator.addMethod("routignumber",
        function (value, element) {
            var n = 0;
            for (var i = 0; i < value.length; i += 3) {
                n += parseInt(value.charAt(i), 10) * 3
                + parseInt(value.charAt(i + 1), 10) * 7
                + parseInt(value.charAt(i + 2), 10);
            }
            return value.match(/^(0[0-9]|1[0-2]|2[1-9]|3[0-2]|6[1-9]|7[0-2]|80)\d{7}$/) && n != 0 && n % 10 == 0;
        },
        "Invalid routing number"
    );
}