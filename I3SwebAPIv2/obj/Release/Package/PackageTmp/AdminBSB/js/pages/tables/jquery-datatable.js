$(function () {
    $('.js-basic-example').DataTable({
        destroy: true,
        responsive: true,
        paging: true,
        searching: true
    });

    //Exportable table
    $('.js-exportable').DataTable({
        dom: 'Bfrtip',
        responsive: true,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
});