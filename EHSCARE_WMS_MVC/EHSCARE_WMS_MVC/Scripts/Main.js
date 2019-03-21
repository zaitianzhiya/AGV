$(document).ready(function () {
    $(".form_date").datetimepicker({
        format: "yyyy-mm-dd",
        showMeridian: 1,
        minView: 2,
        language: 'zh-CN',
        pickerPosition: "bottom-left",
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0
    });

    //$('.ajaxGrid table thead tr a').on('click', updateGrid);
    $('.ajaxGrid table tfoot tr a').on('click', updateGrid);
    $("#divGrid table tbody tr").on("click", function () {
        $(".selectedRow").removeClass("selectedRow");
        $(this).addClass("selectedRow");
    });
});

function updateGrid(e) {
    e.preventDefault();
    var url = $(this).attr('href');
    var grid = $(this).parents('.ajaxGrid');
    var id = grid.attr('id');
    grid.load(url + ' #' + id);
};

function BindClick() {
    $("#divGrid table tbody tr").each(function () {
        $(this).on("click", function () {
            $(".selectedRow").removeClass("selectedRow");
            $(this).addClass("selectedRow");
        });
    });
}