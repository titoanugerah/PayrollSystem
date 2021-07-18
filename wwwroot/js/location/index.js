var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/location/readDatatable",
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    
    "columns": [
        { "data" : "name" },
        { "data" : "district.name" },
        {
            "render": function (data, type, row) {
                return formatter.format(row.umk);
            }
        },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showEditForm('" + row.id + "'); >Edit</button>";
            }
        },
    ]
});

var formatter = new Intl.NumberFormat('id-ID', {
    style: 'currency',
    currency: 'IDR',
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
            $('#editDistrictId').val(result.districtId).change();
            $('#editUMK').val(result.umk);
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
}

function addLocation() {
    if ($("#addName").val() != "" && $("#addUMK").val() != "" && $("#addDistrictId").val() != 0) {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "JSON",
            contentType: "application/x-www-form-urlencoded",
            url: "api/location/create",
            data: {
                Name: $("#addName").val(),
                UMK: $("#addUMK").val(),
                DistrictId: $("#addDistrictId").val(),
            },
            success: function (result) {
                $("#addName").val(""); 
                $("#addUMK").val("");
                $("#addDistrictId").val(0).change();
                $('.spinner-border').hide();
                $('#addLocationModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'Location berhasil ditambahkan', 'success');
                reloadTable();
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#addLocationModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    } else {
        $('#addLocationModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom Nama, UMK atau Distrik", 'danger');
    }
}


function updateLocation() {
    if ($("#editName").val() != "" && $("#editUMK").val() != "" && $("#editDistrictId").val() != 0) {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded",
            url: "api/location/update/" + $('#editId').val(),
            data: {
                Name: $("#editName").val(),
                UMK: $("#editUMK").val(),
                DistrictId: $("#editDistrictId").val(),
            },
            success: function (result) {
                $('.spinner-border').hide();
                $('#editLocationModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'Location berhasil diubah', 'success');
                reloadTable();
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#editLocationModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    } else {
        $('#editLocationModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom Nama, UMK atau Distrik", 'danger');
    }
}

function deleteLocation() {
    $('.delete').show();
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/location/delete/" + $('#editId').val(),
        success: function (result) {
            reloadTable();
            $('.delete').hide();
            $('#editLocationModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Location berhasil dihapus', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.delete').hide();
            $('#editLocationModal').modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function recoverLocation() {
    $('.spinner-border').show();
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/location/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('.spinner-border').hide();
            $('#addLocationModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Location berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.spinner-border').hide();
            $('#addLocationModal').modal('hide');
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

function getDistrict() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/district/read",
        success: function (result) {
            var html = "<option value=0> Silahkan Pilih</option>";
            result.forEach(function (data) {
                html = html + "<option value=" + data.id + "> " + data.name + "</option>";
            });
            $('#addDistrictId').html(html);
            $('#editDistrictId').html(html);
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
    $('.select2editmodal').select2({
        dropdownParent: $('#editLocationModal')
    });
    $('.spinner-border').hide();
    getDeletedLocation();
    getDistrict();
});
