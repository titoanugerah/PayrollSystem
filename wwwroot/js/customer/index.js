
var table = $("#tblCustomer").DataTable({
    "processing": true,
    "serverSide": true,
    ajax: {
        url: "/api/customer/readDatatable",
        dataSrc: 'data',
        dataType: 'json'
    },
    columns: [
        { data: "name" },
        { data: "remark"},
    ]
});

function showAddCustomerForm() {
    $('#modalAddCustomer').modal('show');
}

function reload() {
    table.ajax.reload();
    notify("fa fa-check","Berhasil", "Data berhasil di reload", "success");
}

function addCustomer() {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        url: "api/customer/create",
        data: {
            Name: $("#addName").val(),
            Remark: $("#addRemark").val(),
        },
        success: function (result) {
            console.log(result);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' +result.responseText, 'danger');
        }
    });
}

function getDeletedCustomer() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/customer/readDeleted",
        success: function (result) {
            console.log(result);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.responseText, 'danger');
        }
    });
}

$(document).ready(function () {
    $('.select2addmodal').select2({
        dropdownParent: $('#modalAddCustomer')
    });
    getDeletedCustomer();
});
