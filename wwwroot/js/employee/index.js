var table = $("#tblEmployee").DataTable({
    "dom" : 'Bfrtip',
    "buttons": [
        {
            "title": "Database Driver ",
            "extend": "excelHtml5",
            "exportOptions": {
                "columns": [0, 1, 2, 3, 4, 5],
                "modifier" : {
                    "order" : 'current',
                    "page" : 'all',
                    "selected" : false,
                },
            }
        },
        {
            "title": "Database Driver ",
            "extend": "pdfHtml5",
            "orientation" : "landscape",
            "pageSize" : "LEGAL",
            "exportOptions": {
                "columns": [0, 1, 2, 3, 4, 5],
                "modifier": {
                    "order": 'current',
                    "page": 'all',
                    "selected": false,
                },
            }
        },
        {
            "title": "Database Driver ",
            "extend": "csvHtml5",
            "exportOptions": {
                "columns": [0, 1, 2, 3, 4, 5],
                "modifier": {
                    "order": 'current',
                    "page": 'all',
                    "selected": false,
                },

            }
        },
        {
            "extend": "copyHtml5",
            "exportOptions": {
                "columns": [0, 1, 2, 3, 4, 5]
            }
        },
        {
            "title": "Database Driver ",
            "extend": "print",
            "exportOptions": {
                "columns": [0, 1, 2, 3, 4, 5],
                "modifier": {
                    "order": 'current',
                    "page": 'all',
                    "selected": false,
                },

            }
        },
        "colvis"        
    ],
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering" : false,
    "paging": true,
    "pageLength": 100,
    "ajax" : {
        "url": "/api/employee/readDatatable/0",
        "dataSrc" : "data",
        "type" : "POST",
        "dataType": 'json',
        "error": function (result) {
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    },
    "columns" : [
        {
            "render": function (data,type, row) {
                return zeroPad(row.nik);
            }
        },
        { "data": "name", "name": "Name"},
        { "data": "customer.name",  "name": "Customer" },
        { "data": "position.name", "name": "Position"},
        { "data": "location.name", "name": "Lokasi" },
        { "data": "location.district.name", "name": "Distrik" },
        {
            "render": function (data,type, row) {
                return "<button type='button' class='btn btn-info' onclick=editEmployeeForm('" + row.nik + "'); >Edit</button>";
            }
        },
    ]
});

$('#selectDistrictId').on('change', function () {
    districtId = $('#selectDistrictId').val();
    table.ajax.url("/api/employee/readDatatable/"+districtId).load();
});

function showAddEmployeeForm() {
    $('#addEmployeeModal').modal('show');
}

function addEmployee() {
    $('.spinner-border').show();
    var fd = new FormData();
    var files = $('#fileUpload1')[0].files[0];
    fd.append('file', files);
    $.ajax({
        url: 'api/employee/create/',
        data: fd,
        processData: false,
        contentType: false,
        type: 'post',
        success: function (response) {
            reloadTable()
            $('.spinner-border').hide();
            $('#addEmployeeModal').modal('hide');
           
        },
        error: function (result) {
            reloadTable()
            $('.spinner-border').hide();
            $('#addEmployeeModal').modal('hide');
            if (result.responseText == "0") {
                $('#errorAddEmployeeModal').modal('show');
            }
            else {
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        }
    });
}

function getBank() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/bank/read",
        success: function (result) {
            var html = "<option value=''> Silahkan Pilih </option>";
            result.forEach(function (data) {
                html = html + "<option value='"+data.code+"'>"+data.name+"</option>";
            });
            $('#editBankCode').html(html);
        }
    });
}

function getPosition() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/position/read",
        success: function (result) {
            var html = "<option value=''> Silahkan Pilih </option>";
            result.forEach(function (data) {
                html = html + "<option value='" + data.id+ "'>" + data.name + "</option>";
            });
            $('#editPositionId').html(html);
        }
    });
}

function getLocation() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/location/read",
        success: function (result) {
            var html = "<option value=''> Silahkan Pilih </option>";
            result.forEach(function (data) {
                html = html + "<option value='" + data.id + "'>" + data.name + "</option>";
            });
            $('#editLocationId').html(html);
        }
    });
}

function getDistrict() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/district/read/",
        success: function (result) {
            var html = "<option value='0'> Semua </option>";
            result.forEach(function (data) {
                html = html + "<option value='" + data.id + "'>" + data.name + "</option>";
            });
            $('#selectDistrictId').html(html);
        }
    });
}

function getRole() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/role/read",
        success: function (result) {
            var html = "<option value=''> Silahkan Pilih </option>";
            result.forEach(function (data) {
                html = html + "<option value='" + data.id + "'>" + data.name + "</option>";
            });
            $('#editRoleId').html(html);
        }
    });
}

function updateEmployee() {
    $('.spinner-border').show();
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/x-www-form-urlencoded",
        url: "api/employee/update/" + $('#editNIK').val(),
        data: {
            Name: $("#editName").val(),
            PhoneNumber: $('#editPhoneNumber').val(),
            KTP: $('#editKTP').val(),
            KK: $('#editKK').val(),
            NPWP: $('#editNPWP').val(),
            JamsostekNumber: $('#editJamsostekNumber').val(),
            JamsostekRemark: $('#editJamsostekRemark').val(),
            BpjsNumber: $('#editBpjsNumber').val(),
            BpjsRemark: $('#editBpjsRemark').val(),
            DriverLicense: $('#editDriverLicense').val(),
            DriverLicenseType: $('#editDriverLicenseType').val(),
            AccountNumber: $('#editAccountNumber').val(),
            AccountName: $('#editAccountName').val(),
            BankCode: $('#editBankCode').val(),
            FamilyStatusCode: $('#editFamilyStatusCode').val(),
            PositionId: $('#editPositionId').val(),
            CustomerId: $('#editCustomerId').val(),
            LocationId: $('#editLocationId').val(),
            RoleId: $('#editRoleId').val(),
            IsExist: $('#editIsExist').val()
        },
        success: function (result) {
            reloadTable();
            $('.spinner-border').hide();
            $('#editEmployeeModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Pekerja berhasil diubah', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.spinner-border').hide();
            $('#editEmployeeModal').modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function getCustomer() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/customer/read",
        success: function (result) {
            var html = "<option value=''> Silahkan Pilih </option>";
            result.forEach(function (data) {
                html = html + "<option value='" + data.id + "'>" + data.name + "</option>";
            });
            $('#editCustomerId').html(html);
        }
    });
}
function editEmployeeForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/employee/readDetail/" + id,
        success: function (result) {
            $('#editNIK').val(zeroPad(result.nik));
            $('#editName').val(result.name);
            $('#editPhoneNumber').val(result.phoneNumber);
            $('#editKTP').val(result.ktp);
            $('#editNPWP').val(result.npwp);
            $('#editJamsostekNumber').val(result.jamsostekNumber);
            $('#editJamsostekRemark').val(result.jamsostekRemark);
            $('#editBpjsNumber').val(result.bpjsNumber);
            $('#editBpjsRemark').val(result.bpjsRemark);
            $('#editDriverLicense').val(result.driverLicense);
            $('#editDriverLicenseType').val(result.driverLicenseType);
            $('#editAccountNumber').val(result.accountNumber);
            $('#editAccountName').val(result.accountName);
            $('#editBankCode').val(result.bankCode).change();
            $('#editFamilyStatusCode').val(result.familyStatusCode).change();

            $('#editPositionId').val(result.positionId).change();
            $('#editCustomerId').val(result.customerId).change();
            $('#editLocationId').val(result.locationId).change();
            $('#editRoleId').val(result.roleId).change();


        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
    $('#editEmployeeModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    getBank();
}

function zeroPad(num) {
    return num.toString().padStart(4, "0");
}

$(document).ready(function () {
    $('.spinner-border').hide();
    getBank();
    getLocation();
    getPosition();
    getCustomer();
    getDistrict();
    getRole();

});
