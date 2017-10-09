$(document).ready(function () {
    var admin = new Admin();
    admin.Start();
});

function Admin() {
    var self = this;
    this.cacheEnable = false;

    this.Start = function () {
        self.OnListUsers();
        self.OnListBuildings();
        self.OnListBuildingLots();
        self.OnListMakes();
        self.OnMontlyStalls();
        self.OnVisitorStalls();
        self.OnAllocatedStalls();
    };

    this.OnListUsers = function () {
        $('#UsersTable').dataTable({
            "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
            "sPaginationType": "bootstrap",
            "bSort": false,
            "bServerSide": true,
            "sAjaxSource": "ListUsers",
            "bProcessing": true,
            "bAutoWidth": false,
            "oLanguage": {
                "sLengthMenu": "_MENU_ records per page"
            },
            "aoColumns": [
                            {
                                "mData": function (source, type, val) {
                                    return source.FirstName + ' ' + source.LastName;
                                },
                            },
                            {
                                "mData": "EmailAddress"
                            },
                            {
                                "mData": "UserName"
                            },
                            {
                                "bSearchable": false,
                                "mData": "UserProfileTypeDescription"
                            },
                            {
                                "bSearchable": false,
                                "mData": "UserProfileStatusDescription"
                            },
                            {
                                "mData": null,
                                "bSearchable": false,
                                "fnRender": function (oObj) {
                                    var id = oObj.aData.UserProfileID;
                                    return '<a title=\"Details\" href=\"/Admin/ResetPassword/' + id + '\">Reset Password</a>';
                                }
                            },
                            {
                                "mData": null,
                                "bSearchable": false,
                                "bSortable": false,
                                "fnRender": function (oObj) {
                                    var id = oObj.aData.UserProfileID;
                                    return '<a title=\"Edit\" href=\"/Admin/EditUser/' + id + '\"><i class="icon-edit"></i></a>' +
                                           '<a title=\"Delete\" href=\"/Admin/DeleteUser/' + id + '\"><i class="icon-remove"></i></a>';
                                }
                            }
            ]
        });
    };
    
    this.OnListBuildings = function () {
        var BuildingID;
        $('#BuildingsTable').dataTable({
            "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
            "sPaginationType": "bootstrap",
            "bSort": false,
            "bAutoWidth": false,
            "oLanguage": {
                "sLengthMenu": "_MENU_ records per page"
            },
            "bServerSide": true,
            "sAjaxSource": "ListBuildings",
            "bProcessing": true,
            "aoColumns": [
                            {
                                "mData": "BuildingName",
                                "bSortable": false,
                                "fnRender": function renderLink(oObj) {
                                    BuildingID = oObj.aData.BuildingID;
                                    return "<a href='/Admin/EditBuilding/"
                                        + BuildingID + "'>" + oObj.aData.BuildingName + "</a>";
                                }
                            },
                            {
                                "mData": "OwnerID",
                                "bSortable": false,
                                "fnRender": function render(oObj) {
                                    return oObj.aData.Owner.FirstName + ' ' + oObj.aData.Owner.LastName;
                                }
                            },
                            {
                                "mData": "ManagerID",
                                "fnRender": function render(oObj) {
                                    return "TBD";
                                }
                            },
                            {
                                "mData": "BuildingAddress.City"
                            },
                            {
                                "mData": null, //IsActive
                                "fnRender": function (oObj) { return renderBool(oObj.aData.IsActive); }
                            },
                            {
                                "mData": null, // Tenants
                                "fnRender": function (oObj) {
                                    return "<a href='/Admin/Tenants/" + BuildingID + "'>View tenants</a>";
                                }
                            },
                            {
                                "mData": null, //ParkingLots
                                "fnRender": function (oObj) {
                                    return "<a href='/Admin/Lots/" + BuildingID + "'>View lots</a>";

                                }
                            },
                            {
                                "mData": "BuildingID",
                                "bSearchable": false,
                                "bSortable": false,
                                "fnRender": function renderButton(oObj) {
                                    BuildingID = oObj.aData.BuildingID;
                                    return '<a title=\"Edit\" href=\"/Admin/EditBuilding/' +
                                                        BuildingID + '\"><i class="icon-edit"></i></a>' +
                                                        '<a title=\"Delete\" href=\"/Admin/DeleteBuilding/' +
                                                            BuildingID + '\"><i class="icon-remove"></i></a>';
                                }
                            }
            ]
        });
    };
 
    this.OnListBuildingLots = function () {
        $('#BuildingLotsTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },
    	"bServerSide": true,
    	"sAjaxSource": "ListBuildingLots", 
    	"bProcessing": true,
    	"aoColumns": [
                        {
                            "mData": "LotID",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function renderButton(oObj) {
                                return '';
                            }
                        },
			            {
			                "mData": "LotName",
			                "bSortable": false,
			                "fnRender": function renderLink (oObj) {
			                    return oObj.aData.LotName;
			                }
			            }
		            ]                       
        });
    };

    this.OnListMakes = function () {
        $('#VehicleMakesTable').dataTable({
            "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
            "sPaginationType": "bootstrap",
            "bSort": false,
            "bServerSide": true,
            "sAjaxSource": "ListMakes",
            "bProcessing": true,
            "bAutoWidth": false,
            "oLanguage": {
                "sLengthMenu": "_MENU_ records per page"
            },
            "aoColumns": [
                            {
                                "mData": "VehicleMake1"
                            },
                            {
                                "mData": "VehicleMakeDescription"
                            },
                            {
                                "mData": "isActive"
                            },
                            {
                                "mData": "UpdatedDate",
                                "fnRender": function (oObj, pDate) { return renderDate(oObj, pDate) }
                            },
			                {
			                    "mData": "CreatedDate",
			                    "fnRender": function (oObj, pDate) { return renderDate(oObj, pDate) }
			                },
                            {
                                "mData": "VehicleMakeID",
                                "bSearchable": false,
                                "bSortable": false,
                                "fnRender": function renderButton(oObj) {
                                    return '<a title=\"Edit\" href=\"/Admin/EditMake/' + oObj.aData.VehicleMakeID +'\"><i class="icon-edit"></i></a>' +
                                                        '<a title=\"Delete\" href=\"/Admin/DeleteMake/' +
                                                        oObj.aData.VehicleMakeID + '\"><i class="icon-remove"></i></a>' +
                                             '<a title=\"Models\" href=\"/Admin/Models/' + oObj.aData.VehicleMakeID + '\"><i class="icon-models"></i></a>';
                                }
                            }
            ]
        });
    };

    //Fill Makes Data for add new.--------------------------------
    $("#AddMakeForm").validate();

    $('#AddMakeForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        $("#VehicleMake1").rules("add", { required: true, minlength: 1, messages: { required: "*", minlength: "*" } });

        if ($('#AddMakeForm').valid()) {
            $('#AddMakeForm').submit();
        }
        return false;
    });

    $('#EditMakeForm').validate();

    $('#EditMakeForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        $("#VehicleMake1").rules("add", { required: true, minlength: 1, messages: { required: "*", minlength: "*" } });

        if ($('#EditMakeForm').valid()) {
            $('#EditMakeForm').submit();
        }
        return false;
    });

    $('#EditModelForm').validate();

    $('#EditModelForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        $("#VehicleModel1").rules("add", { required: true, minlength: 1, messages: { required: "*", minlength: "*" } });

        if ($('#EditModelForm').valid()) {
            $('#EditModelForm').submit();
        }
        return false;
    });

    
    $('#EditValidationBookForm').validate();

    $('#EditValidationBookForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        $("#BookName").rules("add", { required: true, minlength: 1, messages: { required: "*", minlength: "*" } });
        $("#TicketCount").rules("add", { required: true, number: true, minlength: 1, messages: { required: "*", number: "*", minlength: "*" } });
        $("#Rate").rules("add", { required: true, number: true,  minlength: 1, messages: { required: "*", number:"*",  minlength: "*" } });

        if ($('#EditValidationBookForm').valid()) {
            $('#EditValidationBookForm').submit();
        }
        return false;
    });

    $('#EditKeyCardForm').validate();

    $('#EditKeyCardForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        $("#KeyCardName").rules("add", { required: true, minlength: 1, messages: { required: "*", minlength: "*" } });
        $("#Rate").rules("add", { required: true, number: true, minlength: 1, messages: { required: "*", number: "*", minlength: "*" } });

        if ($('#EditKeyCardForm').valid()) {
            $('#EditKeyCardForm').submit();
        }
        return false;
    });

    //BUILDING ------------------------------------- 

    $('#iSMailing').click(function (event) {
        var same = this.checked;
        $('#MailingAddress_Address1').val(same ? $('#BuildingAddress_Address1').val() : '');
        $('#MailingAddress_Address2').val(same ? $('#BuildingAddress_Address2').val() : '');
        $('#MailingAddress_City').val(same ? $('#BuildingAddress_City').val() : '');
        $('#MailingAddress_StateID').val(same ? $('#BuildingAddress_StateID').val() : '');
        $('#MailingAddress_ZipCode').val(same ? $('#BuildingAddress_ZipCode').val() : '');
        $('#Building_MailingPhoneNumber').val(same ? $('#Building_PrimaryPhoneNumber').val() : '');
        $('#Building_MailingFaxNumber').val(same ? $('#Building_PrimaryFaxNumber').val() : '');
        $('#mailingList').setReadOnly(same, 'input:text');
        $('#mailingList').setReadOnly(same, 'select');
    });

    $("#editBuildingForm").validate();

    $('#editBuildingForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        $("#Building_BuildingName").rules("add", { required: true, maxlength: 256, messages: {required: "*", maxlength: "*"} });
        $("#Building_OwnerID").rules("add", { required: true, messages: { required: "*" } });
        $("#BuildingAddress_Address1").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
        $("#BuildingAddress_ZipCode").rules("add", { required: true, digits: true, minlength: 6, maxlength: 10, messages: { required: "*", minlength: "*", maxlength: "*" } });
        $("#BuildingAddress_City").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
        $("#BuildingAddress_StateID").rules("add", { required: true, messages: { required: "*" } });
        $("#Building_PrimaryPhoneNumber").rules("add", { required: true, messages: { required: "*" } });
        $("#Building_BankName").rules("add", { required: true, messages: { required: "*" } });
        $("#Building_AccountName").rules("add", { required: true, messages: { required: "*" } });
        $("#Building_AccountNumber").rules("add", { required: true, messages: { required: "*" } });
        $("#Building_RoutingNumber").rules("add", { required: true, messages: { required: "*" } });
        $("#Building_DueDateReminder").rules("add", { required: true, messages: { required: "*" } });
        $("#Building_ReminderFrequency").rules("add", { required: true, messages: { required: "*" } });
        $("#Building_ReminderCutoff").rules("add", { required: true, messages: { required: "*" } });

        if (!$('#iSMailing').is(':checked')) {
            $("#MailingAddress_Address1").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
            $("#MailingAddress_ZipCode").rules("add", { required: true, digits: true, minlength: 6, maxlength: 10, messages: { required: "*", digits:"*", minlength: "*", maxlength: "*" } });
            $("#MailingAddress_City").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
            $("#MailingAddress_StateID").rules("add", { required: true, messages: { required: "*" } });
            $("#Building_MailingPhoneNumber").rules("add", { required: true, messages: { required: "*" } });
        }
        else {
            $("#MailingAddress_Address1").rules("remove");
            $("#MailingAddress_ZipCode").rules("remove");
            $("#MailingAddress_City").rules("remove");
            $("#MailingAddress_StateID").rules("remove");
            $("#Building_MailingPhoneNumber").rules("remove");
        }

        if ($('#editBuildingForm').valid()) {
            $('#editBuildingForm').submit();
        }
        return false;
    });

    //-------------------------------------------

    //STALL ---------------------------------------

    this.OnMontlyStalls = function () {
        var montlyStallsID;
        var LotID = window.GetLotID();
        $('#MontlyStallTable').dataTable({
            "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
            "sPaginationType": "bootstrap",
            "bSort": false,
            "bAutoWidth": false,
            "oLanguage": {
                "sLengthMenu": "_MENU_ records per page"
            },
            "bServerSide": true,
            "sAjaxSource": "../ListMontlyStalls",
            "bProcessing": true,
            "bAutoWidth": false,
            //"fnServerData": function (sSource, aoData, fnCallback) {
            //    $.getJSON(sSource, aoData, function (json) {
            //        fnCallback(json)
            //    }).complete(self.OnVisitorStalls());
            //},
            "fnServerParams" : function(aoData){
                aoData.push({ "name": "LotID", "value": LotID });
            },
            "aoColumns": [
                            {
                                "mData": "StallLocation",
                            },
                            {
                                "mData": "ParkingStallType.ParkingStallType1",
                                "bSortable": false
                            },
                            {
                                "mData": "StallNumber",
                                "sClass": "align_center",
                            },
                            {
                                "mData": "BookedStalls",
                                "sClass": "align_center",
                                "fnRender": function Link(oObj) {
                                    return "<a href='/Admin/AllocatedStall/" + oObj.aData.ParkingStallID + "'>" + oObj.aData.BookedStalls + "</a>";
                                }
                            },
                            {
                                "mData": "MontlyRate",
                                "sClass": "align_center",
                            },
                            {
                                "mData": "OverSell",
                                "sClass": "align_center",
                            },
                            {
                                "mData": null,
                                "bSearchable": false,
                                "bSortable": false,
                                "fnRender": function renderButton(oObj) {
                                    montlyStallsID = oObj.aData.ParkingStallID;
                                    return '<a title=\"Edit\" href=\"/Admin/EditStall/' +
                                                        montlyStallsID + '?lotid=' + LotID + '\"><i class="icon-edit"></i></a>' +
                                                        '<a title=\"Delete\" href=\"/Admin/DeleteStall?stallId=' +
                                                        montlyStallsID + '&lotId='+ LotID +'\"><i class="icon-remove"></i></a>';
                                }
                            }
            ]
        });
    };

    $("#editStallForm").validate();

    $("#ParkingStallTypeID").change(function (event) {
        if ($("#ParkingStallTypeID").val() == 5) {  //if type visitor
            $("#editStallForm").find('.notVisitor').hide();
            $("#editStallForm").find('.visitor').show();
            $("#notes").removeClass("even");
            $("#notes").addClass("odd");
            $("#MontlyRate").val("");
            $("#OverSell").val("");
        }
        else {
            $("#editStallForm").find('.visitor').hide();
            $("#editStallForm").find('.notVisitor').show();
            $("#notes").removeClass("odd");
            $("#notes").addClass("even");
            $("#Rate").val("");
            $("#FlatRate").val("");
            $("#MaxRate").val("");
            $("#TimeIncrement").val("");
            $("#GracePeriod").val("");
        }

        if ($("#User_UserProfileTypeID").val() == 9) {
            $('#hiredate_box').show();
            $('#isSupervisor_box').css("display","inline-block");//.show();
        }
        else {
            $('#hiredate_box').hide();
            $('#isSupervisor_box').hide();
        }
        return false;
    });

    $('#editStallForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        $("#StallLocation").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
        $("#ParkingStallTypeID").rules("add", { required: true, messages: { required: "*" } });        
        $("#StallNumber").rules("add", { required: true, digits: true, messages: { required: "*", digits: "*" } });
        if ($("#ParkingStallTypeID").val() == 5) {
            $("#Rate").rules("add", { required: true, number: true, messages: { required: "*", number: "*" } });
            $("#FlatRate").rules("add", { number: true, messages: { number: "*" } });
            $("#MaxRate").rules("add", { number: true, messages: { number: "*" } });
            $("#TimeIncrement").rules("add", { digits: true, messages: { digits: "*" } });
            $("#GracePeriod").rules("add", { digits: true, messages: { digits: "*" } });
        }
        else {
            $("#MontlyRate").rules("add", { required: true, number: true, messages: { required: "*", number: "*" } });
            $("#OverSell").rules("add", { number: true, messages: { number: "*" } });
        }

        if ($('#editStallForm').valid()) {
            $('#editStallForm').submit();
        }
        return false;
    });

    //-----------------------------

    this.Empty = function () {
    };
}

function OnListLots() {
    var LotId;
    var LocalBuildingId = window.GetBuildingId();
    $('#LotsTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },
        "bServerSide": true,
        "sAjaxSource": "/Admin/ListLots",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "BuildingId", "value": LocalBuildingId })
        },
        "bProcessing": true,
        "bAutoWidth": false,
        "aoColumns": [
                        {
                            "mData": "LotName",
                        },
                        {
                            "mData": "Building.BuildingName",
                            "bSortable": false
                        },
                        {
                            "mData": null,//"ParkingOperatorID",
                            "fnRender": function render(oObj) {
                                return "TBD";
                            }
                        },
                        {
                            "mData": null,//"ParkingManagerID",
                            "fnRender": function render(oObj) {
                                return "TBD";
                            }
                        },
                        {
                            "mData": "ParkingAttendants",
                        },
                        {
                            "mData": "Address.City"
                        },
                        {
                            "mData": "IsActive",
                            "fnRender": function (oObj) { return oObj.aData.IsActive } // return renderBool(oObj.aData.IsActive) 
                        },
                        {
                            "mData": "Address.Address1",
                            "fnRender": function inventoryLink(oObj) {
                                LotId = oObj.aData.LotID;
                                return "<a href='/Admin/Inventory/" + LotId + "'>View inventory</a>"; // LotId here
                            }
                        },
                        {
                            "mData": "LotID",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function renderButton(oObj) {
                                return '<a title=\"Edit\" href=\"/Admin/EditLot/' + LotId
                                         + '\"><i class="icon-edit"></i></a>' +
                                        '<a title=\"Delete\" href=\"/Admin/DeleteLot/' + LotId
                                         + '\"><i class="icon-remove"></i></a>';
                            }
                        }
        ]
    });
};

function OnEditLot() {
    $('#editLotForm').find('#add_building').click(function (event) {
        event.preventDefault();
        var clone = $('#editLotForm').find('.buildings').last().clone();
        var index = parseInt(clone.attr('name').substring(10)) + 1;
        clone.val('').attr('id', 'buildingId' + index).attr('name', 'buildingId' + index).css('margin-left', '265px');
        $('#buildingArea').append('<label class="error" for="buildingId' + index + '" generated="true" value="' + index + '"></label>');
        $('#buildingArea').append(clone);
        $('#buildingArea').append('<a class="delete_building btn btn-primary" href="#" value="' + index + '">Delete</a>');
        $('a.delete_building').last().bind('click', function (event) {
            event.preventDefault();
            var id = $(this).attr('value');
            $('#editLotForm').find('#buildingId' + id).remove();
            $('#editLotForm').find("label[value='" + id + "']").remove();
            $(this).remove();
        });
    });

    if ($('#Lot_LotID').val() != '') {
        var buildings = $('#BuildingIDString').val().split('$');
        for (var i = 0; i < buildings.length; i++) {
            if(i==0)
            {
                $('#buildingId0').val(buildings[0]);
            }
            else {
                $('#add_building').trigger('click');
                $('#buildingId'+i).val(buildings[i]);
            }
        }
    }
    if ($('#iSBuilding').is(':checked')) {
        $('#addressList').find('input:text').attr('readonly', true).css('opacity', 0.7);
        $('#addressList').find('select').attr('readonly', true).css('opacity', 0.7);
    }
    if ($('#iSLot').is(':checked')) {
        $('#mailingList').find('input:text').attr('readonly', true).css('opacity', 0.7);
        $('#mailingList').find('select').attr('readonly', true).css('opacity', 0.7);
    }

    $('#iSBuilding').click(function (event) {
        var same = this.checked;
        var buildingId = $('#editLotForm').find('#buildingId0').val();
        if (same && buildingId != '') {
            $.ajax({
                url: "/Admin/GetBuilding",
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                data: {
                    "buildingId": buildingId,
                },
                success: function (response) {
                    $('#LotAddress_AddressID').val(response.BuildingAddress.AddressID);
                    $('#LotAddress_Address1').val(response.BuildingAddress.Address1);
                    $('#LotAddress_Address2').val(response.BuildingAddress.Address2);
                    $('#LotAddress_City').val(response.BuildingAddress.City);
                    $('#LotAddress_StateID').val(response.BuildingAddress.StateID);
                    $('#LotAddress_ZipCode').val(response.BuildingAddress.ZipCode);
                    $('#Lot_PhoneNumber').val(response.PrimaryPhoneNumber);
                    $('#Lot_FaxNumber').val(response.PrimaryFaxNumber);
                },
                error: function (XMLHttpRequest, etype, exo) {
                    alert(etype, exo);
                    return;
                }
            });
        }
        else {
            //   $('#LotAddress_AddressID').val();
            $('#LotAddress_Address1').val('');
            $('#LotAddress_Address2').val('');
            $('#LotAddress_City').val('');
            $('#LotAddress_StateID').val('');
            $('#LotAddress_ZipCode').val('');
            $('#Lot_PhoneNumber').val('');
            $('#Lot_FaxNumber').val('');
        }
        $('#addressList').setReadOnly(same, 'input:text');
        $('#addressList').setReadOnly(same, 'select');
    });

    $('#iSLot').click(function (event) {
        var same = this.checked;
        $('#MailingAddress_Address1').val(same ? $('#LotAddress_Address1').val() : '');
        $('#MailingAddress_Address2').val(same ? $('#LotAddress_Address2').val() : '');
        $('#MailingAddress_City').val(same ? $('#LotAddress_City').val() : '');
        $('#MailingAddress_StateID').val(same ? $('#LotAddress_StateID').val() : '');
        $('#MailingAddress_ZipCode').val(same ? $('#LotAddress_ZipCode').val() : '');
        $('#Lot_MailingPhoneNumber').val(same ? $('#Lot_PhoneNumber').val() : '');
        $('#Lot_MailingFaxNumber').val(same ? $('#Lot_FaxNumber').val() : '');
        $('#mailingList').setReadOnly(same, 'input:text');
        $('#mailingList').setReadOnly(same, 'select');
    });

    $("#editLotForm").validate();

    $('#editLotForm').find('[name=save]').click(function (event) {
        event.preventDefault();
        $('#editLotForm').find('.buildings').each(function (index) {
            if (index == 0) {
                $(this).rules("add", { required: true, messages: { required: "*" } });
            }
            else {
                $(this).rules("add", { required: true, notEqualElementsFromClass: ".buildings", messages: { required: "*", notEqualElementsFromClass: "*" } });
            }
        });
        $("#Lot_ParkingOperatorID").rules("add", { required: true, messages: { required: "*" } });
        $("#Lot_ParkingManagerID").rules("add", { required: true, messages: { required: "*" } });
        $("#Lot_LotName").rules("add", { required: true, maxlength: 50, messages: { required: "*", maxlength: "*" } });

        var islot = $('#iSLot').is(':checked');
        if (!islot) {
            $("#MailingAddress_Address1").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
            $("#MailingAddress_ZipCode").rules("add", { required: true, digits: true, minlength: 6, maxlength: 10, messages: { required: "*", digits: "*", minlength: "*", maxlength: "*" } });
            $("#MailingAddress_City").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
            $("#MailingAddress_StateID").rules("add", { required: true, messages: { required: "*" } });
        }
        else {
            $("#MailingAddress_Address1").rules("remove");
            $("#MailingAddress_ZipCode").rules("remove");
            $("#MailingAddress_City").rules("remove");
            $("#MailingAddress_StateID").rules("remove");
        }

        var isbuilding = $('#iSBuilding').is(':checked');
        if (!isbuilding) {
            $("#LotAddress_Address1").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
            $("#LotAddress_ZipCode").rules("add", { required: true, digits: true, minlength: 6, maxlength: 10, messages: { required: "*", digits: "*", minlength: "*", maxlength: "*" } });
            $("#LotAddress_City").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
            $("#LotAddress_StateID").rules("add", { required: true, messages: { required: "*" } });
        }
        else {
            $("#LotAddress_Address1").rules("remove");
            $("#LotAddress_ZipCode").rules("remove");
            $("#LotAddress_City").rules("remove");
            $("#LotAddress_StateID").rules("remove");
        }

        if ($('#editLotForm').valid()) {
            var buildings;
            $('#editLotForm').find('.buildings').each(function (index) {
                buildings = (index == 0 ? $.trim($(this).val()) : buildings + '$' + $.trim($(this).val()));
            });
            $('#BuildingIDString').val(buildings);
            $('#editLotForm').submit();
        }
        return false;
    });

    return false;
};

function OnListModels() {
    var MakeId = window.GetMakeId();
    $('#VehicleModelsTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bServerSide": true,
        "sAjaxSource": "/Admin/ListModels",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "MakeId", "value": MakeId })
        },
        "bProcessing": true,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },

        "aoColumns": [
                        {
                            "mData": "VehicleModel1"
                        },
                        {
                            "mData": "VehicleModelDescription"
                        },
                        {
                            "mData": "isActive"
                        },
                        {
                            "mData": "UpdatedDate",
                            "fnRender": function (oObj, pDate) { return renderDate(oObj, pDate) }
                        },
                        {
                            "mData": "CreatedDate",
                            "fnRender": function (oObj, pDate) { return renderDate(oObj, pDate) }
                        },
                        {
                            "mData": "VehicleModelID",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function renderButton(oObj) {
                                return '<a title=\"Edit\" href=\"/Admin/EditModel/' + oObj.aData.VehicleModelID + '\"><i class="icon-edit"></i></a>';
                                '<a title=\"Delete\" href=\"/Admin/DeleteModel/' +
                                oObj.aData.VehicleModelID + '\"><i class="icon-remove"></i></a>';
                            }
                        }
        ]
    });
}

function OnListTenants() {
    var BuildingId = window.GetBuildingId();
    var CompanyID;
    $('#TenantsTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },
        "bServerSide": true,
        "sAjaxSource": "/Admin/ListTenants",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "BuildingId", "value": BuildingId })
        },
        "bProcessing": true,
        "aoColumns": [
                        {
                            "mData": "Company.CompanyName"
                        },
                        {
                            "mData": null,
                            "fnRender": function render(oObj) {
                                return oObj.aData.ManagerAddress.City + '<br/>' + oObj.aData.ManagerAddress.Address1 + '<br/>' + oObj.aData.ManagerAddress.Address2;
                            }
                        },
                        {
                            "mData": null,
                            "fnRender": function render(oObj) {
                                return oObj.aData.ManagerProfile.FirstName + '<br/>' + oObj.aData.ManagerProfile.LastName;
                            }
                        },
                        {
                            "mData": "ManagerProfile.EmailAddress",
                        },
                        {
                            "mData": null,
                            "fnRender": function renderTrue(oObj) { return "True"; }
                        },
                        {
                            "mData": null,
                            "fnRender": function renderLease(oObj) {
                                CompanyID = oObj.aData.Company.CompanyID;
                                return "<a href='/Admin/TenantLease/" + CompanyID + "'>View</a>";
                            }
                        },
                        {
                            "mData": null,
                            "fnRender": function renderLease(oObj) {
                                return "<a href='/Admin/LeaseAbstract/" + CompanyID + "'>View</a>";
                            }
                        },
                        {
                            "mData": null,
                            "fnRender": function renderLink(oObj) {
                                return '<a title=\"Edit\" href=\"/Admin/Employees/' + CompanyID + '\">View</a>';
                            }
                        }
        ]
    });
};

function OnListEmployees() {
    var CompanyID = window.GetCompanyId();
    $('#TenantsTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },
        "bServerSide": true,
        "sAjaxSource": "/Admin/ListEmployees",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "CompanyID", "value": CompanyID })
        },
        "bProcessing": true,
        "aoColumns": [
                        {
                            "mData": "FirstName",

                        },
                        {
                            "mData": "LastName",

                        }
        ]
    });
};

function OnListKeyCards() {
    var LotId = window.GetLotId();
    $('#KeyCardsTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },
        "bServerSide": true,
        "sAjaxSource": "/Admin/ListKeyCards",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "LotId", "value": LotId })
        },
        "bProcessing": true,
        "aoColumns": [
                        {
                            "mData": "KeyCardName",
                        },
                        {
                            "mData": "Rate",
                        },                    
                        {
                            "mData": null,
                            "fnRender": function render(oObj) {
                                //oObj.aData.BookId;
                                return '<a title=\"Edit\" href=\"/Admin/EditKeyCard?Cardid=' + oObj.aData.KeyCardID + '\"><i class="icon-edit"></i></a>' +
                                '<a title=\"Delete\" href=\"/Admin/DeleteKeyCard/' +
                                 +oObj.aData.KeyCardID + '?Lotid=' + LotId + '\"><i class="icon-remove"></i></a>';    // this links aren't currently avalible
                            }
                        },
        ]
    });
};

function OnListValidationBooks() {
    var LotId = window.GetLotId(); 
    $('#ValidationBooksTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },
        "bServerSide": true,
        "sAjaxSource": "/Admin/ListValidationBooks",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "LotId", "value": LotId })
        },
        "bProcessing": true,
        "aoColumns": [
                        {
                            "mData": "BookName",
                            
                        },
                        {
                            "mData": "TicketCount",
                            
                        },
                        {
                            "mData": "Rate",
                            
                        },
                        {
                            "mData": null,
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function render(oObj) {
                                //oObj.aData.BookId;
                                return '<a title=\"Edit\" href=\"/Admin/EditValidationBook?bookid=' + oObj.aData.BookID + '\"><i class="icon-edit"></i></a>' +
                                '<a title=\"Delete\" href=\"/Admin/DeleteValidationBook/' +
                                 +oObj.aData.BookID + '?Lotid=' + LotId + '\"><i class="icon-remove"></i></a>';    // this links aren't currently avalible
                            }
                        },                        
        ]
    });
};

function OnEditUser() {

    $("#User_UserProfileTypeID").change(function (event) {
        if ($("#User_UserProfileTypeID").val() == 8 || $("#User_UserProfileTypeID").val() == 9) {//Parking Manager, Parking Attendant
            $("#editUserForm").find('.company_fields').hide();
            $('#title_box').removeClass("tborder");
            $('#operator_box').show();
            $('#iSCompany').prop('checked', false);
            $('#li_isCompany').hide();
        }
        else {
            if ($("#User_UserProfileTypeID").val() >= 4 && $("#User_UserProfileTypeID").val() <=7) { //Building owner,Parking Operator,Building Manager,Tenant
                $('#building_box').show();
            }
            else {
                $('#building_box').hide();
            }
            $("#editUserForm").find('.company_fields').show();
            $('#title_box').addClass("tborder");
            $('#operator_box').hide();
            $('#li_isCompany').show();
        }
        if ($("#User_UserProfileTypeID").val() == 9) {
            $('#hiredate_box').show();
            $('#isSupervisor_box').css("display", "inline-block");//.show();
        }
        else {
            $('#hiredate_box').hide();
            $('#isSupervisor_box').hide();
        }
        if ($("#User_UserProfileTypeID").val() == 3) { //employee
            $("#editUserForm").find('.company_fields').hide();
            $('#company_select_area').show();
        }
        else {
            $('#company_select_area').hide();
            $("#User_UserProfileTypeID").val() == '';
        }
        return false;
    });

    if ($('#User_UserProfileID').val() > 0) {
        $('#li_Password').hide();
        $('#li_Password2').hide();
        $("#User_UserProfileTypeID").trigger('change');
        $("#User_UserProfileTypeID").attr('readonly', true);
    }

    $("#editUserForm").validate();
    $("#User_HireDate").glDatePicker();

    $("#add_new_tenant").click(function (event) {
        event.preventDefault();

        //window.open('/Admin/EditUser');
        var newWindow = open('/Admin/EditUser');
        if (newWindow.closed) {
            alert('Window closed!');
        }

        return false;
    });

    $("#iSCompany").click(function (event) {
        var same = this.checked;
        if ($("#User_UserProfileTypeID").val() == 3 && same && $("#Company_CompanyID").val() != '') {
            $.ajax({
                url: '/Admin/GetCompanyAddress',
                type: "POST",
                dataType: "json",
                cache: self.cacheEnable,
                async: false,
                data: {
                    'companyID': $("#Company_CompanyID").val(),
                },
                success: function (response) {
                    if (response.success) {
                        $('#Company_AddressID').val(response.address.AddressID);
                        $('#PersonalAddress_City').val(response.address.City);
                        $('#PersonalAddress_Address1').val(response.address.Address1);
                        $('#PersonalAddress_Address2').val(response.address.Address2);
                        $('#PersonalAddress_StateID').val(response.address.StateID);
                        $('#PersonalAddress_ZipCode').val(response.address.ZipCode);
                    }
                    else {
                        alert(response.error)
                    }
                },
                error: function (XMLHttpRequest, etype, exo) {
                    alert(etype, exo);
                }
            });
        }
        else {
            $('#PersonalAddress_Address1').val(same ? $('#Company_Address_Address1').val() : '');
            $('#PersonalAddress_Address2').val(same ? $('#Company_Address_Address2').val() : '');
            $('#PersonalAddress_City').val(same ? $('#Company_Address_City').val() : '');
            $('#PersonalAddress_StateID').val(same ? $('#Company_Address_StateID').val() : '');
            $('#PersonalAddress_ZipCode').val(same ? $('#Company_Address_ZipCode').val() : '');
        }
            $('#mailingList').setReadOnly(same, $('#PersonalAddress_Address1'));
            $('#mailingList').setReadOnly(same, $('#PersonalAddress_Address2'));
            $('#mailingList').setReadOnly(same, $('#PersonalAddress_City'));
            $('#mailingList').setReadOnly(same, $('#PersonalAddress_StateID'));
            $('#mailingList').setReadOnly(same, $('#PersonalAddress_ZipCode'));
    });

    $('#editUserForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        if ($("#User_UserProfileTypeID").val() != 8 && $("#User_UserProfileTypeID").val() != 9 && $("#User_UserProfileTypeID").val() != 3) {

            $("#Company_CompanyName").rules("add", { required: true, messages: { required: "*" } });
            $("#Company_Address_Address1").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
            $("#Company_Address_ZipCode").rules("add", { required: true, digits: true, minlength: 4, maxlength: 10, messages: { required: "*", digits: "*", minlength: "*", maxlength: "*" } });
            $("#Company_Address_City").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
            $("#Company_Address_StateID").rules("add", { required: true, messages: { required: "*" } });
            $("#Company_WorkPhone_PhoneNumber").rules("add", { required: true, maxlength: 15, messages: { required: "*", maxlength: "*" } });
            $("#User_OperatorID").rules("remove");
            if ($("#User_UserProfileTypeID").val() >= 4 && $("#User_UserProfileTypeID").val() <= 7) { //Building owner,Parking Operator,Building Manager,Tenant
                $("#BuildingID").rules("add", { required: true, messages: { required: "*" } });
            }
            else {
                $("#BuildingID").rules("remove");
            }
        }
        else {

            $("#Company_CompanyName").rules("remove");
            $("#Company_Address_Address1").rules("remove");
            $("#Company_Address_ZipCode").rules("remove");
            $("#Company_Address_City").rules("remove");
            $("#Company_Address_StateID").rules("remove");
            $("#Company_WorkPhone_PhoneNumber").rules("remove");
            if ($("#User_UserProfileTypeID").val() == 3) {
                $("#Company_CompanyID").rules("add", { required: true, messages: { required: "*" } });
            }
            else {
                $("#User_OperatorID").rules("add", { required: true, messages: { required: "*" } });
                $("#Company_CompanyID").rules("remove");
            }
        }

        $("#User_UserProfileTypeID").rules("add", { required: true, messages: { required: "*" } });
        $("#User_TitlePreffix").rules("add", { required: true, messages: { required: "*" } });
        $("#User_FirstName").rules("add", { required: true, maxlength: 10, messages: { required: "*", maxlength: "*" } });
        $("#User_LastName").rules("add", { required: true, maxlength: 10, messages: { required: "*", maxlength: "*" } });
        $("#User_EmailAddress").rules("add", { required: true, email: true, messages: { required: "*", email: "*" } });
        $("#User_UserName").rules("add", { required: true, maxlength: 10, messages: { required: "*", maxlength: "*" } });

        $("#WorkPhone_PhoneNumber").rules("add", { required: true, maxlength: 15, messages: { required: "*", maxlength: "*" } });
        if ($('#User_UserProfileID').val() == '') {
            $("#Password").rules("add", { required: true, minlength: 6, messages: { required: "*", minlength: "*" } });
            $("#Password2").rules("add", { required: true, minlength: 6, equalTo: "#Password", messages: { required: "*", minlength: "*", equalTo: "*" } });
        }

        $("#PersonalAddress_Address1").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
        $("#PersonalAddress_ZipCode").rules("add", { required: true, digits: true, minlength: 4, maxlength: 10, messages: { required: "*", digits: "*", minlength: "*", maxlength: "*" } });
        $("#PersonalAddress_City").rules("add", { required: true, maxlength: 256, messages: { required: "*", maxlength: "*" } });
        $("#PersonalAddress_StateID").rules("add", { required: true, messages: { required: "*" } });

        if ($('#editUserForm').valid()) {
            $('#editUserForm').submit();
        }
        return false;
    });
};

function OnVisitorStalls () {
    var visitorStallID;
    var LotID = window.GetLotID();

    $('#VisitorStallTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },
        "bServerSide": true,
        "sAjaxSource": "../ListVisitorStalls",
        "bProcessing": true,
        "bAutoWidth": false,
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "LotID", "value": LotID });
        },
        "aoColumns": [
                        {
                            "mData": "StallLocation",
                        },
                        {
                            "mData": "ParkingStallType.ParkingStallType1",
                            "bSortable": false
                        },
                        {
                            "mData": "StallNumber",
                            "sClass": "align_center",
                        },
                        {
                            "mData": "BookedStalls",
                            "sClass": "align_center",
                            "fnRender": function Link(oObj) {
                                return "<a href='/Admin/AllocatedStall/" + oObj.aData.ParkingStallID + "'>" + oObj.aData.BookedStalls + "</a>";
                            }
                        },
                        {
                            "mData": "TimeIncrement",
                            "sClass": "align_center",
                        },
                        {
                            "mData": "Rate",
                            "sClass": "align_center",
                        },
                        {
                            "mData": "MaxRate",
                            "sClass": "align_center",
                        },
                        {
                            "mData": "GracePeriod",
                            "sClass": "align_center",
                        },
                        {
                            "mData": "FlatRate",
                            "sClass": "align_center",
                        },
                        {
                            "mData": null,
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function renderButton(oObj) {
                                visitorStallID = oObj.aData.ParkingStallID;
                                return '<a title=\"Edit\" href=\"/Admin/EditStall/' +
                                                   visitorStallID + '?lotid=' + LotID + '\"><i class="icon-edit"></i></a>' +
                                                   '<a title=\"Delete\" href=\"/Admin/DeleteStall?stallId=' +
                                                   visitorStallID + '&lotId='+ LotID +'\"><i class="icon-remove"></i></a>';
                            }
                        }
        ]
    });
};

function OnAllocatedStalls (stallID) {
    var StallID = stallID;

    $('#AllocatedStallTable').dataTable({
        "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
        "sPaginationType": "bootstrap",
        "bSort": false,
        "bAutoWidth": false,
        "oLanguage": {
            "sLengthMenu": "_MENU_ records per page"
        },
        "bServerSide": true,
        "sAjaxSource": "../ListAllocatedStalls",
        "bProcessing": true,
        "bAutoWidth": false,
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "StallID", "value": StallID });
        },
        "aoColumns": [
                        {
                            "mData": "Company.CompanyName",
                        },
                        {
                            "mData": "NumberOfStalls",
                            "sClass": "align_center",
                        },
                        {
                            "mData": "MontlyRate",
                            "sClass": "align_center",
                        },
                        {
                            "mData": null,
                            "sClass": "align_center",
                            "fnRender": function render(oObj) {
                                return "0";
                            }
                        },
                        {
                            "mData": "EffectiveFrom",
                            "fnRender": function (oObj, pDate) { return renderDate(oObj, pDate) }
                        },
                        {
                            "mData": "EffectiveTo",
                            "fnRender": function (oObj, pDate) { return renderDate(oObj, pDate) }
                        }
        ]
    });
};

function OnEditTenantLease() {

    $("#editTenantLeaseForm").validate({
        rules: {
            LateFee: {
                required: true,
                number: true
            },
            TermStart: {
                required: true,
                date: true
            },
            TermEnd: {
                required: true,
                date: true
            },
        },
        messages: {
            LateFee: { required: "*", number: "*" },
            TermStart: { required: "*", date: "*" },
            TermEnd: { required: "*", date: "*" },
        }
    });
    $("#TermStart").glDatePicker();
    $("#TermEnd").glDatePicker();

    $('#editTenantLeaseForm').find('[name=save]').click(function (event) {
        event.preventDefault();



        if ($('#editTenantLeaseForm').valid()) {
            $('#editTenantLeaseForm').submit();
        }
        return false;
    });
};

function OnEditLeaseAbstract() {
    $("#Inventory_EffectiveFrom").glDatePicker();
    $("#Inventory_EffectiveTo").glDatePicker();

    $("#editMontlyParkingForm").validate();
    $("#Inventory_EffectiveFrom").rules("add", { required: true, date: true, messages: { required: "*", date: "*" } });
    $("#Inventory_EffectiveTo").rules("add", { required: true, date: true, messages: { required: "*", date: "*" } });
    $("#Inventory_MontlyRate").rules("add", { required: true, number: true, messages: { required: "*", number: "*" } });
    $("#Inventory_DiscountRate").rules("add", { required: true, number: true, messages: { required: "*", number: "*" } });
    $("#Inventory_NumberOfStalls").rules("add", { required: true, digits: true, messages: { required: "*", digits: "*" } });

    $('#Inventory_MontlyRate').blur(calculate);
    $('#Inventory_NumberOfStalls').blur(calculate);
    $('#Inventory_DiscountRate').blur(calculate);

    $('#editMontlyParkingForm').find('[name=save]').click(function (event) {
        event.preventDefault();

        if ($('#editMontlyParkingForm').valid()) {
            $('#editMontlyParkingForm').submit();
        }
        return false;
    });

    function calculate() {
        var rate;
        var qty;
        var disc;
        rate = $('#Inventory_MontlyRate').val();
        qty = $('#Inventory_NumberOfStalls').val();
        disc = $('#Inventory_DiscountRate').val();
        var effRate = rate - (rate * disc / 100)
        $('#effRate').text(effRate);
        $('#total').text(effRate * qty);
    }
};

function renderDate(oObj, pDate) {
    if (pDate != '') {
        var javascriptDate = new Date(parseInt(pDate.slice(6, -2)));
        javascriptDate = javascriptDate.getMonth() + "/" + javascriptDate.getDate() + "/" + javascriptDate.getFullYear();
        return javascriptDate;
    }
    return '';
};

function renderBool(data) {
    return castStrToBool(data.toString()) ? 'Active' : 'Not active';
};

function castStrToBool(str) {
    if (str.toLowerCase() == 'false') {
        return false;
    } else if (str.toLowerCase() == 'true') {
        return true;
    } else {
        return undefined;
    };
};

(function ($) {
    $.fn.setReadOnly = function (readonly, typeElement) {
        return this.find(typeElement)
        .attr('readonly', readonly)
        .css('opacity', readonly ? 0.7 : 1.0)
    }
})(jQuery)

jQuery.validator.addMethod("notEqualElementsFromClass", function (value, element, param) {
    var same = true;
    $(document).find(param).not(element).each(function (index, item) {
        if ($(item).val() === value ) {
            same = false;
            return false;
        }      
    });
    return this.optional(element) || same;
}, '');