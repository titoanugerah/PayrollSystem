
var table = $("#tblEmployee").DataTable({
    "processing": true,
    "serverSide": true,
    ajax: {
        url: "/api/employee/readDatatable",
        dataSrc: 'data',
        dataType: 'json'
    },
    columns: [
        { data: "nik" },
        { data: "name"},
        { data: "customer.name"},
        { data: "position.name"},
        { data: "location.name"},
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
