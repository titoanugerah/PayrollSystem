var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/payrollDetail/readPersonalDatatable/" + $("#payrollHistoryId").val(),
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        { "data": "employee.nik" },
        { "data": "employee.name" },
        {
            "render": function (data, type, row) {
                return "Rp. " + row.takeHomePay.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            }
        },
        { "data": "payrollDetailStatus" },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showDetailForm('" + row.id + "'); >Detail</button>";
            }
        },
    ]
});
