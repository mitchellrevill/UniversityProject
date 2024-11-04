
const host = `${window.location.protocol}//${window.location.host}`;

function getSelectedIds() {
    const selectedIds = [];
    document.querySelectorAll(".dynamic-checkbox-item:checked").forEach((checkbox) => {
        selectedIds.push(checkbox.dataset.id); // Collect selected IDs
    });
    return selectedIds;
}
populateRegionsOptions()
populateCountryOptionsEdit()
populateManagerOptions()
populateDepartmentOptions()
populateEmployeeTable()
async function populateEmployeeTable() {
    try {
        const Departments = await FetchRequestGET('GetAllEmployees');
        if (!Array.isArray(Departments)) {
            throw new Error('Expected an array from FetchRequestGET.');
        }

        const tbody = document.getElementById('EmployeeTableBody');
        tbody.innerHTML = '';

        if (Departments.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4">No locations available.</td></tr>';
            return;
        }

        Departments.forEach(item => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.EmployeeId}" data-name="${item.FirstName}" data-description="${item.LastName}">${item.EmployeeId}</td>
                <td>${item.FirstName}</td>
                <td>${item.LastName}</td>
                <td>${item.CompanyEmail}</td>
                <td>${item.PersonalEmail}</td>
                <td>${item.PhoneNumber}</td>
            `;

            tbody.appendChild(row);
        });
    } catch (error) {
        console.error('Error populating table:', error);
    }
}

function addNewEmployee() {
    console.log("Method Start");

    
    var employeeId = document.getElementById("employeeId").value;
    var firstName = document.getElementById("firstName").value;
    var lastName = document.getElementById("lastName").value;
    var companyEmail = document.getElementById("companyEmail").value;
    var personalEmail = document.getElementById("personalEmail").value;
    var phoneNumber = document.getElementById("phoneNumber").value;
    var countryId = document.getElementById("CountryOptions1").value;
    var regionId = document.getElementById("RegionOptions").value;
    var departmentId = document.getElementById("DepartmentOptions").value;
    var managerId = document.getElementById("ManagerOptions1").value;
    var employmentType = document.getElementById("employmentType").value;
    var startDate = document.getElementById("startDate").value;
    var salary = document.getElementById("salary").value;
    var benefits = document.getElementById("benefits").value;
    var password = document.getElementById("password").value;
    var employeeType = document.getElementById("employeeType").value;
    var status = document.getElementById("status").value;

    
    if (!employeeId || !firstName || !lastName || !companyEmail || !countryId || !regionId || !departmentId || !startDate || !password) {
        alert("Please fill out all required fields.");
        return;
    }

    
    var employee = {
        "EmployeeId": employeeId,
        "FirstName": firstName,
        "LastName": lastName,
        "CompanyEmail": companyEmail,
        "PersonalEmail": personalEmail,
        "PhoneNumber": phoneNumber,
        "CountryId": countryId,
        "RegionId": regionId,
        "DepartmentId": departmentId,
        "ManagerId": managerId,
        "EmploymentType": employmentType,
        "StartDate": startDate,
        "Salary": parseFloat(salary) || 0,
        "Benefits": benefits,
        "Password": password,
        "EmployeeType": employeeType,
        "Status": status
    };

    // Send the employee data to your server or function
    FetchRequest('insertEmployee', employee);
    alert('Employee added successfully');
}


function UpdateEmployee() {
    console.log("Method Start");
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No countries selected for updating.");
        return;
    }
    selectedIds.forEach(element => {
        var employeeId = document.getElementById("employeeId").value;
        var firstName = document.getElementById("firstName").value;
        var lastName = document.getElementById("lastName").value;
        var companyEmail = document.getElementById("companyEmail").value;
        var personalEmail = document.getElementById("personalEmail").value;
        var phoneNumber = document.getElementById("phoneNumber").value;
        var countryId = document.getElementById("CountryOptions1").value;
        var regionId = document.getElementById("RegionOptions").value;
        var departmentId = document.getElementById("DepartmentOptions").value;
        var managerId = document.getElementById("ManagerOptions1").value;
        var employmentType = document.getElementById("employmentType").value;
        var startDate = document.getElementById("startDate").value;
        var salary = document.getElementById("salary").value;
        var benefits = document.getElementById("benefits").value;
        var password = document.getElementById("password").value;
        var employeeType = document.getElementById("employeeType").value;
        var status = document.getElementById("status").value;


        console.log(element);


        var employee = {
            "EmployeeId": employeeId,
            "FirstName": firstName,
            "LastName": lastName,
            "CompanyEmail": companyEmail,
            "PersonalEmail": personalEmail,
            "PhoneNumber": phoneNumber,
            "CountryId": countryId,
            "RegionId": regionId,
            "DepartmentId": departmentId,
            "ManagerId": managerId,
            "EmploymentType": employmentType,
            "StartDate": startDate,
            "Salary": parseFloat(salary) || 0,
            "Benefits": benefits,
            "Password": password,
            "EmployeeType": employeeType,
            "Status": status
        };
        FetchRequest('UpdateEmployees', employee);
    })

    alert('Employee Updated successfully');
}


function deleteEmployee() {
    console.log("Request received for deleting manager");

    const managerId = document.getElementById("managerIdEdit").value;

    if (!managerId) {
        alert("No manager selected for deletion.");
        return;
    }

    var managerPosting = {
        "ManagerId": managerId,
        "EmployeeId": "NULL",
        "ManagerArea": "NULL"
    };

    FetchRequest('DeleteEmployee', managerPosting);
    alert('Successful')
}


async function FetchRequest(uri, model) {
    console.log("Req sent")
    try {
        const response = await fetch(host + '/' + uri, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(model)
        });

        if (response.ok) {
            const data = await response.text();
        } else {
            throw new Error('Error performing operation');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation');
    }
}


async function populateDepartmentOptions() {
    try {
        const response = await FetchRequestGET('GetAllDepartments');


        const departments = Array.isArray(response) ? response : response.data || [];


        const selectElements = document.querySelectorAll('.department-options');

        console.log(selectElements); 


        selectElements.forEach(select => {
            select.innerHTML = ''; 

            
            departments.forEach(item => {
                if (item.DepartmentId && item.DepartmentName) { // Ensure values exist
                    const option = document.createElement('option');
                    option.value = item.DepartmentId;
                    option.textContent = item.DepartmentName;
                    select.appendChild(option);
                } else {
                    console.warn('Undefined DepartmentId or DepartmentName for item:', item);
                }
            });
        });
    } catch (error) {
        console.error("Error fetching or populating departments:", error);
    }
}

async function populateCountryOptionsEdit() {
    const Countries = await FetchRequestGET('GetCountries');
    const selectElements = document.querySelectorAll('.country-options');

    selectElements.forEach(select => {

        select.innerHTML = '';

        Countries.forEach(item => {
            const option = document.createElement('option');
            option.value = item.CountryId;
            option.textContent = item.CountryName;

            select.appendChild(option);
        });
    });
}

async function populateRegionsOptions() {
    const Countries = await FetchRequestGET('GetAllRegions');
    const selectElements = document.querySelectorAll('.Region-options');

    console.log(selectElements)
    selectElements.forEach(select => {

        select.innerHTML = '';

        Countries.forEach(item => {
            const option = document.createElement('option');
            option.value = item.RegionId;
            option.textContent = item.RegionName;

            select.appendChild(option);
        });
    });
}

async function populateManagerOptions() {
    const Countries = await FetchRequestGET('GetAllEmployees');
    const selectElements = document.querySelectorAll('.manager-options');
    console.log("Request received for updating manager");
    console.log(selectElements)
    selectElements.forEach(select => {

        select.innerHTML = '';

        Countries.forEach(item => {
            const option = document.createElement('option');
            option.value = item.EmployeeId;
            option.textContent = item.FirstName;

            select.appendChild(option);
        });
    });
}



async function FetchRequestGET(uri) {
    try {
        const response = await fetch(host + '/' + uri, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        if (response.ok) {
            const data = await response.json();
            return data
        } else {
            throw new Error('Error performing operation');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation');
    }
}