var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    ajax: {
        url: "/api/district/readDatatable",
        dataSrc: 'data',
        dataType: 'json'
    },
    columns: [
        { data: "name" },
        { data: "remark" },
        { data: "button" },
    ]
});

function showAddDistrictForm() {
    $('#addDistrictModal').modal('show');
}

function showEditForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/district/readDetail/" + id,
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
    $('#editDistrictModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    getDeletedDistrict();
    notify("fa fa-check", "Berhasil", "Data berhasil di reload", "success");
}

function addDistrict() {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        contentType: "application/x-www-form-urlencoded",
        url: "api/district/create",
        data: {
            Name: $("#addName").val(),
            Remark: $("#addRemark").val(),
        },
        success: function (result) {
            $('#addDistrictModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'District berhasil ditambahkan', 'success');
            reloadTable();
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}


function updateDistrict() {
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/x-www-form-urlencoded",
        url: "api/district/update/" + $('#editId').val(),
        data: {
            Name: $("#editName").val(),
            Remark: $("#editRemark").val(),
        },
        success: function (result) {
            $('#editDistrictModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'District berhasil diubah', 'success');
            reloadTable();
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function deleteDistrict() {
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        //dataType: "JSON",
        url: "api/district/delete/" + $('#editId').val(),
        success: function (result) {
            reloadTable();
            $('#editDistrictModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'District berhasil dihapus', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function recoverDistrict() {
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        //dataType: "JSON",
        url: "api/district/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('#addDistrictModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'District berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function getDeletedDistrict() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/district/readDeleted",
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
        dropdownParent: $('#addDistrictModal')
    });
    getDeletedDistrict();
});
