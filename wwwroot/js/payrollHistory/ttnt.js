var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/payrollHistory/readDatatable/"+ $("#mainCustomerId").val(),
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        {
            "render": function (data, type, row) {
                return  row.month + ", " + row.year;
            }
        },
        { "data": "status" },
        {
            "render": function (data, type, row) {
                return "<a href='payrollDetail?id="+row.id+"' class='btn btn-info' >Detail</a>";
            }
        },
    ]
});

function showAddPayrollHistoryForm() {
    $('#addPayrollHistoryModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    //getDeletedPayrollHistory();
}

function addPayrollHistory() {
    if ($("#addMonth").val() != "" && $("#addYear").val() != "" ) {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "JSON",
            contentType: "application/x-www-form-urlencoded",
            url: "api/payrollHistory/create/",
            data: {
                Month: $("#addMonth").val(),
                Year: $("#addYear").val(),
                MainCustomerId : $("#mainCustomerId").val(),
            },
            success: function (result) {
                $('.spinner-border').hide();
                $('#addPayrollHistoryModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'PayrollHistory berhasil ditambahkan', 'success');
                reloadTable();
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#addPayrollHistoryModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    } else {
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom bulan dan tahun", 'danger');
    }
}

$(document).ready(function () {
    $('.select2addmodal').select2({
        dropdownParent: $('#addPayrollHistoryModal')
    });
    $('.spinner-border').hide();
});

