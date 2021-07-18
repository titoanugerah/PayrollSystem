var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/district/readDatatable",
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        { "data": "name" },
        { "data": "remark" },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showEditForm('" + row.id + "'); >Edit</button>";
            }
        },
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
}

function addDistrict() {
    if ($("#addName").val() != "") {
        $('.spinner-border').show();
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
                $('.spinner-border').hide();
                $('#addDistrictModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'District berhasil ditambahkan', 'success');
                reloadTable();
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#addDistrictModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    } else {
        $('#addDistrictModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom nama", 'danger');
    }
}


function updateDistrict() {
    if ($("#editName").val() != "") {
        $('.spinner-border').show();
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
                $('.spinner-border').hide();
                $('#editDistrictModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'District berhasil diubah', 'success');
                reloadTable();
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#editDistrictModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    } else {
        $('#editDistrictModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom nama", 'danger');
    }
}

function deleteDistrict() {
    $('.delete').show();
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/district/delete/" + $('#editId').val(),
        success: function (result) {
            reloadTable();
            $('.delete').show();
            $('#editDistrictModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'District berhasil dihapus', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.delete').show();
            $('#editDistrictModal').modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function recoverDistrict() {
    $('.spinner-border').show();
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/district/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('.spinner-border').hide();
            $('#addDistrictModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'District berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.spinner-border').hide();
            $('#addDistrictModal').modal('hide');
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
    $('.spinner-border').hide();
    getDeletedDistrict();
});
