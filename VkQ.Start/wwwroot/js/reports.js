let scroller = new Scroller('/Reports/GetReports', '.elements', '#filter')
scroller.Start();

$('#clearFilter').click(function () {
    scroller.ResetData()
    return false;
});