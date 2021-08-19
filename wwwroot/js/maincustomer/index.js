var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/maincustomer/readdatatable",
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        { "data": "name"},
        { "data": "remark"},
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showEditForm('" + row.id + "'); >Edit</button>";
            }
        },
    ]
});

function showAddMainCustomerForm() {
    $('#addMainCustomerModal').modal('show');
}

function showEditForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/maincustomer/readDetail/"+id,
        success: function (result) {
            $('#editId').val(result.id);
            $('#editName').val(result.name);
            $('#editRemark').val(result.remark);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
    $('#editMainCustomerModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    getDeletedMainCustomer();
}

function addMainCustomer() {
    if ($("#addName").val()!="") {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "JSON",
            contentType: "application/x-www-form-urlencoded",
            url: "api/maincustomer/create",
            data: {
                Name: $("#addName").val(),
                Remark: $("#addRemark").val(),
            },
            success: function (result) {
                reloadTable();
                $("#addName").val("");
                $("#addRemark").val("");
                $('.spinner-border').hide();
                $('#addMainCustomerModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'Customer berhasil ditambahkan', 'success');
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#addMainCustomerModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' +result.responseText, 'danger');
            }
        });
    } else {
        $('#addMainCustomerModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom nama" , 'danger');
    }
}


function updateMainCustomer() {
    if ($("#editName").val() != "") {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded",
            url: "api/maincustomer/update/"+$('#editId').val(),
            data: {
                Name: $("#editName").val(),
                Remark: $("#editRemark").val(),
            },
            success: function (result) {
                reloadTable();
                $('.spinner-border').hide();
                $('#editMainCustomerModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'Customer berhasil diubah', 'success');
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#editMainCustomerModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    } else {
        $('#editMainCustomerModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom nama", 'danger');
    }

}

function deleteMainCustomer() {
    $('.delete').show();
    $.ajax({
        type: "POST",   
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/maincustomer/delete/"+$('#editId').val(),
        success: function (result) {
            reloadTable();
            $('.delete').hide();
            $('#editMainCustomerModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Customer Utama berhasil dihapus', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.delete').hide();
            $('#editMainCustomerModal').modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function recoverMainCustomer() {
    $('.spinner-border').show();
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/maincustomer/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('.spinner-border').hide();
            $('#addMainCustomerModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Customer Utama berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.spinner-border').hide();
            $('#addMainCustomerModal').modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function getDeletedMainCustomer() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/maincustomer/readDeleted",
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
        dropdownParent: $('#addMainCustomerModal')
    });
    $('.spinner-border').hide();
    getDeletedMainCustomer();
});
