$(document).ready(function () {
    var dashBoard = new DashBoard();
    dashBoard.Start();
});

function DashBoard() {
    var self = this;
    this.cacheEnable = false;

    this.common = new Common();
    this.alerts = new Alerts(self.common);
    this.personalData = new PersonalData(self.common, self.alerts);
    this.vehicles = new Vehicles(self.common, self.alerts);
    this.phones = new Phones(self.common);
    this.payments = new Payments(self.common, self.alerts);

    this.Start = function () {

        $('body').addClass('has-js');
        $('.label_check, .label_radio').click(function () {
            self.common.setupLabel();
        });
        self.common.setupLabel();

        $.validator.setDefaults({
            debug: true
        });

        $("select").selectbox();
    };

    this.Empty = function () {
    };
}