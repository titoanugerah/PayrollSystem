var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    ajax: {
        url: "/api/customer/readDatatable",
        dataSrc: 'data',
        dataType: 'json'
    },
    columns: [
        { data: "name" },
        { data: "remark" },
        { data: "button" },
    ]
});

function showAddCustomerForm() {
    $('#addCustomerModal').modal('show');
}

function showEditForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/customer/readDetail/"+id,
        success: function (result) {
            console.log(result);
            $('#editId').val(result.id);
            $('#editName').val(result.name);
            $('#editRemark').val(result.remark);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
    $('#editCustomerModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    getDeletedCustomer();
    notify("fa fa-check","Berhasil", "Data berhasil di reload", "success");
}

function addCustomer() {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        contentType: "application/x-www-form-urlencoded",
        url: "api/customer/create",
        data: {
            Name: $("#addName").val(),
            Remark: $("#addRemark").val(),
        },
        success: function (result) {
            $('#addCustomerModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Customer berhasil ditambahkan', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' +result.responseText, 'danger');
        }
    });
}


function updateCustomer() {
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/x-www-form-urlencoded",
        url: "api/customer/update/"+$('#editId').val(),
        data: {
            Name: $("#editName").val(),
            Remark: $("#editRemark").val(),
        },
        success: function (result) {
            $('#editCustomerModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Customer berhasil diubah', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function deleteCustomer() {
    $.ajax({
        type: "POST",   
        contentType: 'application/json; charset=utf-8',
        //dataType: "JSON",
        url: "api/customer/delete/"+$('#editId').val(),
        success: function (result) {
            reloadTable();
            $('#editCustomerModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Customer berhasil dihapus', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function recoverCustomer() {
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        //dataType: "JSON",
        url: "api/customer/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('#addCustomerModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Customer berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function getDeletedCustomer() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/customer/readDeleted",
        success: function (result) {
            var html = "<option value=0> Silahkan Pilih</option>";
            result.forEach(function (data) {
                html = html + "<option value=" + data.id + "> " + data.name + "</option>";
            });
            $('#recoverId').html(html);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.responseText, 'danger');
        }
    });
}

$(document).ready(function () {
    $('.select2addmodal').select2({
        dropdownParent: $('#addCustomerModal')
    });
    getDeletedCustomer();
});
