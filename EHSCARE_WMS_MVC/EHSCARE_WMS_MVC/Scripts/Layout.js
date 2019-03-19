$(document).ready(function () {
    var form = $("#txtForm").val();
    if (form == "U" || form == "G" || form == "S" || form == "W") {
        $(".treeview:eq(0)").addClass("active menu-open");
        switch (form) {
            case "U":
                $(".base:eq(0)").addClass("active");
                break;
            case "G":
                $(".base:eq(1)").addClass("active");
                break;
            case "S":
                $(".base:eq(2)").addClass("active");
                break;
            case "W":
                $(".base:eq(3)").addClass("active");
                break;
        }
    }
    else if (form == "I" || form == "O" || form == "B" || form == "E" || form == "R") {
        $(".treeview:eq(1)").addClass("active menu-open");
        switch (form) {
            case "I":
                $(".fun:eq(0)").addClass("active");
                break;
            case "O":
                $(".fun:eq(1)").addClass("active");
                break;
            case "R":
                $(".fun:eq(2)").addClass("active");
                break;
            case "B":
                $(".fun:eq(3)").addClass("active");
                break;
            case "E":
                $(".fun:eq(4)").addClass("active");
                break;
        }
    }
    else if (form == "P") {
        $("#liPri").addClass("active");
    }
})