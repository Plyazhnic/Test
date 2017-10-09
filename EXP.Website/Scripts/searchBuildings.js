$(document).ready(function () {
    $('#btnSave11').click(function (event) {
        event.preventDefault();
        var locCommon = new Common();  

        var model = new Object();
        model.BuildingName = $('#searchCriteria').find('#txtBuildingName').val();
        model.OwnersFirstName = $('#searchCriteria').find('#txtOwnersFirstName').val();
        model.OwnersLastName = $('#searchCriteria').find('#txtOwnersLastName').val();
        model.ManagerFirstName = $('#searchCriteria').find('#txtManagerFirstName').val();
        model.ManagerLastName = $('#searchCriteria').find('#txtManagerLastName').val();
        model.City = $('#searchCriteria').find('#txtCity').val();
        model.StateID = $('#searchCriteria').find('#StateID').val();
        model.ZipCode = $('#searchCriteria').find('#txtZipCode').val();

        $.ajax({
            url: window.searchBuildingsUrl,
            type: "POST",
            dataType: 'json',
            cache: false,
            data: model,

            success: function (response) {
                jQuery.each(response.buildings, function () {
                    //$('#lstResult').find('tr').remove();
                    $('#lstResult > tbody:last').append('<tr>...</tr><tr>...</tr>');
                    //self.RemoveAlert(this);
                });
            },

            error: function (XMLHttpRequest, etype, exo) {
                locCommon.ShowErrorBox(etype, exo);
            }
        });

        return false;
    });
});
