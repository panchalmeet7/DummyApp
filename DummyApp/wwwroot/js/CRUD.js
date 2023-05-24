//AddEmployeeForm validations
$(document).ready(function () {
    $("#AddEmployeeForm").validate({

        rules: {
            firstname: "required",
            lastname: "required",
            email: {
                required: true,
                email: true
            },
            role: "required",
            position: "required",
            department: "required",
            status: "required"

        },

        messages: {
            firstname: "Please enter your firstname",
            lastname: "Please enter your lastname",
            role: "Please enter your role",
            position: "Please enter your position",
            department: "Please enter your department",
            status: "Please enter your status"

        }

    });
});

//Add Employee Data
function AddEmployee() {
    if ($("#AddEmployeeForm").valid()) {
        console.log("AddEmployee function call");
        var firstname = $('#firstname').val();
        var lastname = $('#lastname').val();
        var email = $("#email").val();
        var role = $('#role').val();
        var position = $('#position').val();
        var department = $('#department').val();
        var status = $('#status').val();
        $.ajax({
            url: "/CRUD/AddEmployeeData",
            type: "POST",
            data: {
                firstname: firstname,
                lastname: lastname,
                email: email,
                role: role,
                position: position,
                department: department,
                status: status
            },
            success: function (data) {
                console.log(data);
                toastr.success('Data Added Successfully', 'Success', { timeOut: 3000 });
                window.setTimeout(
                    function () {
                        location.reload()
                    }, 3000);
            },
            error: function (e) {
                console.log(e);
            }
        })
    }

}


//EditEmployeeForm validations
$(document).ready(function () {
    $("#EditEmployeeForm").validate({

        rules: {
            efirstname: "required",
            elastname: "required",
            eemail: {
                required: true,
                email: true
            },
            erole: "required",
            eposition: "required",
            edepartment: "required",
            estatus: "required"

        },

        messages: {
            firstname: "Please enter your firstname",
            lastname: "Please enter your lastname",
            role: "Please enter your role",
            position: "Please enter your position",
            department: "Please enter your department",
            status: "Please enter your status"

        }

    });
});


//Edit Employee Data
//function EditEmployee() {
//    if ($("#EditEmployeeForm").valid()) {
//        console.log("EditEmployee function call");
//        var firstname = $('#firstname').val();
//        var lastname = $('#lastname').val();
//        var email = $("#email").val();
//        var role = $('#role').val();
//        var position = $('#position').val();
//        var department = $('#department').val();
//        var status = $('#status').val();
//        $.ajax({
//            url: "/CRUD/AddEmployeeData",
//            type: "POST",
//            data: {
//                firstname: firstname,
//                lastname: lastname,
//                email: email,
//                role: role,
//                position: position,
//                department: department,
//                status: status
//            },
//            success: function (data) {
//                console.log(data);
//                location.reload();
//            },
//            error: function (e) {
//                console.log(e);
//            }
//        })
//    }

//}


//Retrive Employee Data

function RetriveEmployeeData(EmployeeId) {
    //console.log(EmployeeId);
    $.ajax({
        url: "/CRUD/GetSingleEmployeeRecord",
        type: "GET",
        data: {
            EmployeeId: EmployeeId
        },
        success: function (data) {
            // console.log(data);
            $.each(data.data, function (key, value) {
                $("#efirstname").val(value.employeeFirstName);
                $("#elastname").val(value.employeeLastName);
                $("#eemail").val(value.employeeEmail);
                $("#erole").val(value.employeeRole);
                $("#edepartment").val(value.employeeDepartment);
                $("#estatus").val(value.status);
                $("#eposition").val(value.position);
            });
        },
        error: function (e) {
            console.log(e);
        }
    })
}


function DeleteEmployeeData(EmployeeId) {
    console.log(EmployeeId);
    $.ajax({
        url: "/CRUD/DeleteEmployeeData",
        type: "POST",
        data: {
            EmployeeId: EmployeeId
        },
        success: function (data) {
            console.log(data);
            toastr.success('Data Deleted Successfully', 'Success', { timeOut: 1000 });
            window.setTimeout(
                function () {
                    location.reload()
                }, 1000);
        },
        error: function (data) {
            console.log(data);
            toastr.error('Error che bhai', 'ERROR', { timeOut: 3000 });
        }
    })
}