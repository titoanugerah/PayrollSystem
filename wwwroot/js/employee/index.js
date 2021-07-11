
var table = $("#tblEmployee").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering" : false,
    "paging": true,
    "pageLength": 10,
    "ajax" : {
        "url": "/api/employee/readDatatable",
        "dataSrc" : "data",
        "type" : "POST",
        "dataType": 'json'
    },
    "columns" : [
        { "data": "nik", "name": "NIK"},
        { "data": "name", "name": "Name"},
        { "data": "customer.name",  "name": "Customer" },
        { "data": "position.name", "name": "Position"},
        { "data": "location.name", "name": "Location" },
        {
            "render": function (data,type, row) {
                return "<button type='button' class='btn btn-info' onclick=editEmployeeForm('" + row.nik + "'); >Edit</button>";
            }
        },
    ]
});

function showAddEmployeeForm() {
    $('#addEmployeeModal').modal('show');
}

function addEmployee() {
    var fd = new FormData();
    var files = $('#fileUpload1')[0].files[0];
    //var files = $('#fileUpload1').files;
    fd.append('file', files);
    $.ajax({
        url: 'api/employee/create/',
        data: fd,
        processData: false,
        contentType: false,
        type: 'post',
        success: function (response) {
            reload()
            $('#addEmployeeModal').modal('hide');
            console.log('success', response);
        },
        error: function (result) {
            console.log('error', result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function reload() {
    table.ajax.reload();
    notify("fa fa-check","Berhasil", "Data berhasil di reload", "success");
}

$(document).ready(function () {

});
