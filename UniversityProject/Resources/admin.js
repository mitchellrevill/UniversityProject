function getSelectedIds() {
    const selectedIds = [];
    document.querySelectorAll(".dynamic-checkbox-item:checked").forEach((checkbox) => {
        selectedIds.push(checkbox.dataset.id);  // Collect selected IDs
    });
    return selectedIds;
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

    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No departments selected for updating.");
        return;
    }
    selectedIds.forEach(element => {
    var postingId =  element
    var DepartmentTitle = document.getElementById("departmentName").value;
    var DepartmentDescription = document.getElementById("DepartmentDescription").value;

    
    if (!DepartmentTitle || !DepartmentDescription) {
        alert("You have not answered all required fields");
        return;
    }

    
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
    });
    
}



async function DeleteDepartment() {
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No departments selected for updating.");
        return;
    }
    selectedIds.forEach(element => {
        var postingId =  element
        var DepartmentTitle = document.getElementById("departmentName").value;
        var DepartmentDescription = document.getElementById("DepartmentDescription").value;
    
        
        if (!DepartmentTitle || !DepartmentDescription) {
            alert("You have not answered all required fields");
            return;
        }
    
        
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
                    throw new Error('Error deleting the Department posting');
                }
            })
            .then(data => {
                alert(data); // Show success message
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Failed to deleting Department posting');
            }); 
    });
}

addCountry()
EditCountry()
DeleteCountry()

addRegion()
EditRegion()
DeleteRegion()