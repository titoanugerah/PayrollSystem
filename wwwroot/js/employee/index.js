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
    console.log("/api/employee/readDatatable/1");
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
            console.log('success', response);
        },
        error: function (result) {
            reloadTable()
            console.log('error', "Terdapat beberapa data yang error, silahkan download dan perbaiki");
            $('.spinner-border').hide();
            $('#addEmployeeModal').modal('hide');
            $('#errorAddEmployeeModal').modal('show');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
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
            BirthPlace: $('#editBirthPlace').val(),
            BirthDate: $('#editBirthDate').val(),
            Sex: $('#editSex').val(),
            Religion: $('#editReligion').val(),
            Address: $('#editAddress').val(),
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
            DriverLicenseExpire: $('#editDriverLicenseExpire').val(),
            AccountNumber: $('#editAccountNumber').val(),
            AccountName: $('#editAccountName').val(),
            BankCode: $('#editBankCode').val(),
            FamilyStatusCode: $('#editFamilyStatusCode').val(),
            EmploymentStatusId: $('#editEmploymentStatusId').val(),
            PositionId: $('#editPositionId').val(),
            CustomerId: $('#editCustomerId').val(),
            LocationId: $('#editLocationId').val(),
            RoleId: $('#editRoleId').val(),
            StartContract: $('#editStartContract').val(),
            EndContract: $('#editEndContract').val(),
            JoinCompanyDate: $('#editJoinCompanyDate').val(),
            JoinCustomerDate: $('#editJoinCustomerDate').val(),
            HasUniform: $('#editHasUniform').val(),
            UniformDeliveryDate: $('#editUniformDeliveryDate').val(),
            HasIdCard: $('#editHasIdCard').val(),
            IdCardDeliveryDate: $('#editIdCardDeliveryDate').val(),
            HasTraining: $('#editHasTraining').val(),
            TrainingDeliveryDate: $('#editTrainingDeliveryDate').val(),
            TrainingName: $('#editTrainingName').val(),
            TrainingRemark: $('#editTrainingRemark').val(),
            TrainingGrade: $('#editTrainingGrade').val(),
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

            $('#editBirthPlace').val(result.birthPlace);
            $('#editBirthDate').val(result.birthDate.substring(0, 10));
            $('#editSex').val(result.sex).change();
            $('#editReligion').val(result.religion).change();
            $('#editAddress').val(result.address);
            $('#editPhoneNumber').val(result.phoneNumber);
            $('#editKTP').val(result.ktp);
            $('#editKK').val(result.kk);
            $('#editNPWP').val(result.npwp);
            $('#editJamsostekNumber').val(result.jamsostekNumber);
            $('#editJamsostekRemark').val(result.jamsostekRemark);
            $('#editBpjsNumber').val(result.bpjsNumber);
            $('#editBpjsRemark').val(result.bpjsRemark);
            $('#editDriverLicense').val(result.driverLicense);
            $('#editDriverLicenseType').val(result.driverLicenseType);
            $('#editDriverLicenseExpire').val(result.driverLicenseExpire.substring(0,10));
            $('#editAccountNumber').val(result.accountNumber);
            $('#editAccountName').val(result.accountName);
            $('#editBankCode').val(result.bankCode).change();
            $('#editFamilyStatusCode').val(result.familyStatusCode).change();

            $('#editEmploymentStatusId').val(result.employmentStatusId).change();
            $('#editPositionId').val(result.positionId).change();
            $('#editCustomerId').val(result.customerId).change();
            $('#editLocationId').val(result.locationId).change();
            $('#editRoleId').val(result.roleId).change();

            $('#editStartContract').val(result.startContract.substring(0, 10));
            $('#editEndContract').val(result.endContract.substring(0, 10));
            $('#editJoinCompanyDate').val(result.joinCompanyDate.substring(0, 10));
            $('#editJoinCustomerDate').val(result.joinCustomerDate.substring(0, 10));
            if (result.hasUniform) {
                $('#editHasUniform').val("true").change();
            }
            else {
                $('#editHasUniform').val("false").change();
            }
            if (result.isExist) {
                $('#editIsExist').val("true").change();
            }
            else {
                $('#editIsExist').val("false").change();
            }

            $('#editUniformDeliveryDate').val(result.uniformDeliveryDate.substring(0, 10));
            if (result.hasIdCard) {
                $('#editHasIdCard').val("true").change();
            }
            else {
                $('#editHasIdCard').val("false").change();
            }

            $('#editIdCardDeliveryDate').val(result.idCardDeliveryDate.substring(0, 10));

            if (result.hasTraining) {
                $('#editHasTraining').val("true").change();
            }
            else {
                $('#editHasTraining').val("false").change();
            }
            $('#editTrainingDeliveryDate').val(result.trainingDeliveryDate.substring(0, 10));
            $('#editTrainingName').val(result.trainingName);
            $('#editTrainingRemark').val(result.trainingRemark);
            $('#editTrainingGrade').val(result.trainingGrade);

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
    console.log(num);
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
