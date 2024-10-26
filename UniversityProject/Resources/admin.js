
function getCheckedValues() {
    const checkboxes = document.querySelectorAll(".dynamic-checkbox");
    checkboxes.forEach((checkbox) => {
        if (checkbox.checked) {
            console.log("Checked value:", checkbox.value);
        }
    });
}


function addNewDepartment(uri) {

    var postingId =  Math.floor(Math.random() * 1000) + 1;
    var DepartmentTitle = document.getElementById("departmentName").value;
    var DepartmentDescription = document.getElementById("DepartmentDescription").value;

    // Validate required fields
    if (!DepartmentTitle || !DepartmentDescription) {
        alert("You have not answered all required fields");
        return;
    }

    // Define the DepartmentPosting object before using it
    var DepartmentPosting = {
        "DepartmentId": postingId,
        "DepartmentName": DepartmentTitle,
        "Description": DepartmentDescription,
    };

    var host = window.location.host;
    fetch(host + '/' + uri, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(DepartmentPosting)
    })
        .then(response => {
            if (response.ok) {
                return response.text(); // Success message
            } else {
                throw new Error('Error inserting the Department posting');
            }
        })
        .then(data => {
            alert(data); // Show success message
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Failed to insert Department posting');
        });
}

function Update(uri) {

    var postingId =  Math.floor(Math.random() * 1000) + 1;
    var DepartmentTitle = document.getElementById("departmentName").value;
    var DepartmentDescription = document.getElementById("DepartmentDescription").value;

    // Validate required fields
    if (!DepartmentTitle || !DepartmentDescription) {
        alert("You have not answered all required fields");
        return;
    }

    // Define the DepartmentPosting object before using it
    var DepartmentPosting = {
        "DepartmentId": postingId,
        "DepartmentName": DepartmentTitle,
        "Description": DepartmentDescription,
    };

    var host = window.location.host;
    fetch(host + '/' + uri, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(DepartmentPosting)
    })
        .then(response => {
            if (response.ok) {
                return response.text(); // Success message
            } else {
                throw new Error('Error inserting the Department posting');
            }
        })
        .then(data => {
            alert(data); // Show success message
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Failed to insert Department posting');
        });
}

updateDepartment()
DeleteDepartment()

addCountry()
EditCountry()
DeleteCountry()

addRegion()
EditRegion()
DeleteRegion()