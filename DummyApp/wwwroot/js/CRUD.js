// Loader
document.onreadystatechange = function () {
    if (document.readyState !== "complete") {
        document.querySelector("body").style.visibility = "hidden";
        document.querySelector("#loader").style.visibility = "visible";
    } else {
        document.querySelector("#loader").style.display = "none";
        document.querySelector("body").style.visibility = "visible";
    }
};

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
        $('#SaveChangesBtn').text("Saving");
        $('#SaveChangesBtn').attr("disabled", true);
        //if (value.innerHTML == "Save") { value.innerHTML = "Saving"; }
        //else { value.innerHTML = "Save"; }
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
                debugger;
                /*$("#EmployeeTablediv").html(data);*/
                $("#SaveChangesBtn").text('Save');
                $('#SaveChangesBtn').attr("disabled", false);
                toastr.success('Data Added Successfully', 'Success', { timeOut: 1000 });
                window.setTimeout(
                    function () {
                        location.reload()
                    }, 1000);
            },
            error: function (e) {
                console.log(e);
            }
        })
    }

}

//Update Employee Data

function UpdateEmployeeData() {
    console.log("thyu thyu");

    $('#UpdateChangesBtn').text("Updating"); //changing the btn text whilst ajax call
    $('#UpdateChangesBtn').attr("disabled", true); // disabling the btn 
    var firstname = $('#efirstname').val();
    var empid = $('#hidempid').val();
    var lastname = $('#elastname').val();
    var email = $("#eemail").val();
    var role = $('#erole').val();
    var position = $('#eposition').val();
    var department = $('#edepartment').val();
    var status = $('#estatus').val();
    $.ajax({
        url: "/CRUD/UpdateEmployeeData",
        type: "POST",
        data: {
            empid: empid,
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
            //$("#EmployeeTablediv").html(data);
            $('#UpdateChangesBtn').text("Updated"); // changing the btn text after ajax success
            $('#UpdateChangesBtn').attr("disabled", false); // enabling the btn
            $('#EditEmployeeModal').modal('hide'); // hidding the modal
            toastr.success('Data Updated Successfully', 'Success', { timeOut: 1500 });
            window.setTimeout(
                function () {
                    location.reload()
                }, 1500);
        },
        error: function (e) {
            console.log(e);
        }
    })


}


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
            //console.log(data);
            if (data.data.length == 0) {
                alert("Error While Getting User Details ");
            }
            else {
                $.each(data.data, function (key, value) {
                    $("#efirstname").val(value.employeeFirstName);
                    $("#hidempid").val(value.employeeId);
                    $("#elastname").val(value.employeeLastName);
                    $("#eemail").val(value.employeeEmail);
                    $("#erole").val(value.employeeRole);
                    $("#edepartment").val(value.employeeDepartment);
                    $("#estatus").val(value.status);
                    $("#eposition").val(value.position);
                });
            }
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