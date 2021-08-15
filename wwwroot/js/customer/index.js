var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/customer/readDatatable",
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        { "data": "name"},
        { "data": "mainCustomer.name"},
        { "data": "remark"},
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showEditForm('" + row.id + "'); >Edit</button>";
            }
        },
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
            $('#editId').val(result.id);
            $('#editName').val(result.name);
            $('#editRemark').val(result.remark);
            $('#editMainCustomerId').val(result.mainCustomerId).change();
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
}

function addCustomer() {
    if ($("#addName").val()!="") {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "JSON",
            contentType: "application/x-www-form-urlencoded",
            url: "api/customer/create",
            data: {
                Name: $("#addName").val(),
                Remark: $("#addRemark").val(),
                MainCustomerId: $("#addMainCustomerId").val()
            },
            success: function (result) {
                reloadTable();
                $("#addName").val("");
                $("#addRemark").val("");
                $('.spinner-border').hide();
                $('#addCustomerModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'Customer berhasil ditambahkan', 'success');
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#addCustomerModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' +result.responseText, 'danger');
            }
        });
    } else {
        $('#addCustomerModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom nama" , 'danger');
    }
}


function updateCustomer() {
    if ($("#editName").val() != "") {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded",
            url: "api/customer/update/"+$('#editId').val(),
            data: {
                Name: $("#editName").val(),
                Remark: $("#editRemark").val(),
                MainCustomerId: $("#editMainCustomerId").val(),
            },
            success: function (result) {
                reloadTable();
                $('.spinner-border').hide();
                $('#editCustomerModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'Customer berhasil diubah', 'success');
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#editCustomerModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    } else {
        $('#editCustomerModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom nama", 'danger');
    }

}

function deleteCustomer() {
    $('.delete').show();
    $.ajax({
        type: "POST",   
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/customer/delete/"+$('#editId').val(),
        success: function (result) {
            reloadTable();
            $('.delete').hide();
            $('#editCustomerModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Customer berhasil dihapus', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.delete').hide();
            $('#editCustomerModal').modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function recoverCustomer() {
    $('.spinner-border').show();
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/customer/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('.spinner-border').hide();
            $('#addCustomerModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Customer berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.spinner-border').hide();
            $('#addCustomerModal').modal('hide');
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

function getMainCustomer() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/maincustomer/read",
        success: function (result) {
            var html = "<option value=0> Silahkan Pilih</option>";
            result.forEach(function (data) {
                html = html + "<option value=" + data.id + "> " + data.name + "</option>";
            });
            $('#addMainCustomerId').html(html);
            $('#editMainCustomerId').html(html);
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
    $('.select2editmodal').select2({
        dropdownParent: $('#editCustomerModal')
    });
    $('.spinner-border').hide();
    getDeletedCustomer();
    getMainCustomer();
});
