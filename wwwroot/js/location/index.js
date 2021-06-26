var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    ajax: {
        url: "/api/location/readDatatable",
        dataSrc: 'data',
        dataType: 'json'
    },
    columns: [
        { data: "name" },
        { data: "remark" },
        { data: "button" },
    ]
});

function showAddLocationForm() {
    $('#addLocationModal').modal('show');
}

function showEditForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/location/readDetail/" + id,
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
    $('#editLocationModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    getDeletedLocation();
    notify("fa fa-check", "Berhasil", "Data berhasil di reload", "success");
}

function addLocation() {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        contentType: "application/x-www-form-urlencoded",
        url: "api/location/create",
        data: {
            Name: $("#addName").val(),
            Remark: $("#addRemark").val(),
        },
        success: function (result) {
            $('#addLocationModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Location berhasil ditambahkan', 'success');
            reloadTable();
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}


function updateLocation() {
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/x-www-form-urlencoded",
        url: "api/location/update/" + $('#editId').val(),
        data: {
            Name: $("#editName").val(),
            Remark: $("#editRemark").val(),
        },
        success: function (result) {
            $('#editLocationModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Location berhasil diubah', 'success');
            reloadTable();
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function deleteLocation() {
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        //dataType: "JSON",
        url: "api/location/delete/" + $('#editId').val(),
        success: function (result) {
            reloadTable();
            $('#editLocationModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Location berhasil dihapus', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function recoverLocation() {
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        //dataType: "JSON",
        url: "api/location/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('#addLocationModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Location berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function getDeletedLocation() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/location/readDeleted",
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
        dropdownParent: $('#addLocationModal')
    });
    getDeletedLocation();
});
