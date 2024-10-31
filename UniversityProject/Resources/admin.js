const host = `${window.location.protocol}//${window.location.host}`;

function getSelectedIds() {
    const selectedIds = [];
    document.querySelectorAll(".dynamic-checkbox-item:checked").forEach((checkbox) => {
        selectedIds.push(checkbox.dataset.id); // Collect selected IDs
    });
    return selectedIds;
}

populateDepartmentTable();
populateCountryTable();
populateRegionTable();
populateLocationTable()
populateRegionsOptions()
populateCountryOptionsEdit()
populateManagerOptions()
// CountryOptions
// employeeIdEdit



async function populateDepartmentTable() {
    try {
        const Departments = await FetchRequestGET('GetAllDepartments');
        if (!Array.isArray(Departments)) {
            throw new Error('Expected an array from FetchRequestGET.');
        }

        const tbody = document.getElementById('departmentTableBody');
        tbody.innerHTML = '';

        if (Departments.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4">No locations available.</td></tr>';
            return;
        }

        Departments.forEach(item => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.DepartmentId}" data-name="${item.DepartmentName}" data-description="${item.Description}"></td>
                <td>${item.DepartmentId}</td>
                <td>${item.DepartmentName}</td>
                <td>${item.Description}</td>
            `;

            tbody.appendChild(row);
        });
    } catch (error) {
        console.error('Error populating department table:', error);
    }
}

async function populateRegionTable() {
    Regions = await FetchRequestGET('GetAllRegions')
    const tbody = document.getElementById('regionTableBody');
    tbody.innerHTML = '';
    if (!Array.isArray(Regions)) {
        throw new Error('Expected an array from FetchRequestGET.');
    }

    if (Regions.length === 0) {
        tbody.innerHTML = '<tr><td colspan="4">No locations available.</td></tr>';
        return;
    }

    Regions.forEach(item => {
        const row = document.createElement('tr');

        row.innerHTML = `
            <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.RegionId}" data-name="${item.RegionName}" data-country-id="${item.CountryId}"></td>
            <td>${item.RegionId}</td>
            <td>${item.RegionName}</td>
            <td>${item.CountryId}</td>
        `;

        tbody.appendChild(row);
    });
}

    async function populateLocationTable() {
        try {

            const Locations = await FetchRequestGET('GetLocations');
            const tbody = document.getElementById('LocationTableBody');
            tbody.innerHTML = ''; 

            if (!Array.isArray(Locations)) {
                throw new Error('Expected an array from FetchRequestGET.');
            }

            if (Locations.length === 0) {
                tbody.innerHTML = '<tr><td colspan="4">No locations available.</td></tr>'; 
                return;
            }

            Locations.forEach(item => {
                const row = document.createElement('tr');

                row.innerHTML = `
            <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.LocationId}" data-name="${item.LocationName}" data-country-id="${item.CountryId}" aria-label="Select ${item.LocationName}"></td>
            <td>${item.LocationId}</td>
            <td>${item.LocationName}</td>
            <td>${item.CountryId}</td>
            `;

                tbody.appendChild(row);
            });
        } catch (error) {
            console.error('Error populating location table:', error);

        }
    }


async function populateCountryTable() {

    Countries = await FetchRequestGET('GetCountries');


    if (Countries.length === 0) {
        tbody.innerHTML = '<tr><td colspan="4">No locations available.</td></tr>';
        return;
    }

    const tbody = document.getElementById('countryTableBody');
    tbody.innerHTML = '';
    console.log(typeof Countries)

    Countries.forEach(item => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.CountryId}" data-name="${item.CountryName}" data-currency="${item.CountryCurrency}" data-minimum-leave="${item.MinimumLeave}"></td>
            <td>${item.CountryId}</td>
            <td>${item.CountryName}</td>
            <td>${item.CountryCurrency}</td>
            <td>${Array.isArray(item.LegalRequirements) ? item.LegalRequirements.join(", ") : ""}</td>
            <td>${item.MinimumLeave}</td>        
          `;
        tbody.appendChild(row);
    });
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



// MANAGER BUTTONS
function addNewManager() {
    console.log("Method Start");

    var managerId = Math.floor(Math.random() * 1000) + 1;
    var employeeId = document.getElementById("employeeIdAdd").value;
    var managerArea = document.getElementById("managerAreaAdd").value;

    if (!employeeId || !managerArea) {
        alert("You have not answered all required fields");
        return;
    }

    var manager = {
        "ManagerId": managerId,
        "EmployeeId": employeeId,
        "ManagerArea": managerArea
    };

    FetchRequest('InsertManager', manager);
    alert('Successful')
}

function updateManager() {
    console.log("Request received for updating manager");

    const employeeId = document.getElementById("employeeIdEdit").value;
    const managerArea = document.getElementById("managerAreaEdit").value;
    const managerId = document.getElementById("managerIdEdit").value;

    if (!employeeId || !managerArea || !managerId) {
        alert("You have not answered all required fields");
        return;
    }

    var managerPosting = {
        "ManagerId": managerId,
        "EmployeeId": employeeId,
        "ManagerArea": managerArea
    };

    FetchRequest('UpdateManager', managerPosting);
    alert('Successful')
}

function deleteManager() {
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

    FetchRequest('DeleteManager', managerPosting);
    alert('Successful')
}

// Location

function addNewLocation() {
    console.log("Method Start");


    var LocationId = Math.floor(Math.random() * 1000) + 1;



    var locationName = document.getElementById("locationName").value;
    var regionId = document.getElementById("RegionOptions").value;
    var countryId = document.getElementById("CountryOptions1").value;
    var latitude = document.getElementById("latitude").value;
    var longitude = document.getElementById("longitude").value;


    if (!locationName || !regionId || !countryId || !latitude || !longitude) {
        alert("You have not answered all required fields");
        return;
    }

    var Location = {
        "LocationId": LocationId,
        "LocationName": locationName,
        "RegionId": regionId,
        "CountryId": countryId,
        "Latitude": parseFloat(latitude),
        "Longitude": parseFloat(longitude)
    };
    console.log("Location object:", Location);

    FetchRequest('InsertLocation', Location);
    alert('Successful')
    populateLocationTable()
}

function updateLocation() {
    console.log("Request received for updating Location");

    const locationId = document.getElementById("locationId").value;
    const locationName = document.getElementById("locationNameEdit").value;
    const regionId = document.getElementById("RegionOptionsEdit").value;
    const countryId = document.getElementById("CountryOptionsEdit").value;
    const latitude = document.getElementById("latitudeEdit").value;
    const longitude = document.getElementById("longitudeEdit").value;

    
    if (!locationId || !locationName || !regionId || !countryId || !latitude || !longitude) {
        alert("You have not answered all required fields");
        return;
    }

    var LocationPosting = {
        "LocationId": locationId,
        "LocationName": locationName,
        "RegionId": regionId,
        "CountryId": countryId,
        "Latitude": parseFloat(latitude),
        "Longitude": parseFloat(longitude)
    };


    FetchRequest('UpdateLocation', LocationPosting);
    alert('Successful')
    populateLocationTable()
}

function deleteLocation() {
    console.log("Request received for deleting Location");


    const locationId = document.getElementById("locationIdDelete").value;


    if (!locationId) {
        alert("No Location selected for deletion.");
        return;
    }


    var LocationPosting = {
        "LocationId": locationId,
        "LocationName": "NULL",
        "RegionId": "NULL",
        "CountryId": "NULL",
        "Latitude": "NULL",
        "Longitude": "NULL"
    };


    FetchRequest('DeleteLocation', LocationPosting);
    alert('Successful')
    populateLocationTable()
}

// Departments
function addNewDepartment() {
    console.log("Method Start");
    var DepartmentId = Math.floor(Math.random() * 1000) + 1;
    var DepartmentTitle = document.getElementById("departmentName").value;
    var DepartmentDescription = document.getElementById("DepartmentDescription").value;

    console.log(DepartmentId)
    if (!DepartmentTitle || !DepartmentDescription) {
        alert("You have not answered all required fields");
        return;
    }

    var Department = {
        "DepartmentId": DepartmentId,
        "DepartmentName": DepartmentTitle,
        "Description": DepartmentDescription,
    };
    FetchRequest('InsertDepartment', Department);
    alert('Successful')
    populateDepartmentTable();
}

function updateDepartment() {
    console.log("Req Rec")
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No departments selected for updating.");
        return;
    }
    console.log("Req Rec")
    selectedIds.forEach(element => {
        var DepartmentId = element;
        var DepartmentTitle = document.getElementById("departmentNameUpdate").value;
        var DepartmentDescription = document.getElementById("departmentDescriptionUpdate").value;

        console.log(element)

        var DepartmentPosting = {
            "DepartmentId": DepartmentId,
            "DepartmentName": DepartmentTitle,
            "Description": DepartmentDescription,
        };
        console.log(DepartmentPosting)
        FetchRequest('UpdateDepartment', DepartmentPosting);
        alert('Successful')
        populateDepartmentTable();
    });
}

async function DeleteDepartment() {
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No departments selected for deletion.");
        return;
    }

    selectedIds.forEach(element => {
        var DepartmentId = element;

        var DepartmentPosting = {
            "DepartmentId": DepartmentId,
            "DepartmentName": "NULL",
            "Description": "NULL",
        };

        FetchRequest('DeleteDepartment', DepartmentPosting);
        alert('Successful')
        populateDepartmentTable();
    });
}



// Country BUTTONS
function addNewCountry() {
    console.log("Method Start");
    var CountryId = Math.floor(Math.random() * 1000) + 1;
    var CountryName = document.getElementById("countryName").value;
    var CountryCurrency = document.getElementById("countryCurrency").value;
    var LegalRequirements = document.getElementById("legalRequirements").value;
    var MinimumLeave = parseInt(document.getElementById("minimumLeave").value);

    if (!CountryId || !CountryName || !CountryCurrency || !LegalRequirements || isNaN(MinimumLeave)) {
        alert("You have not answered all required fields");
        return;
    }

    var Country = {
        "CountryId": CountryId,
        "CountryName": CountryName,
        "CountryCurrency": CountryCurrency,
        "LegalRequirements": LegalRequirements.split(',').map(item => item.trim()), // Convert to array
        "MinimumLeave": MinimumLeave
    };

    FetchRequest('InsertCountry', Country);
    alert('Successful')
    populateCountryTable();
}

function updateCountry() {
    console.log("Request Received");
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No countries selected for updating.");
        return;
    }

    selectedIds.forEach(element => {
        var CountryId = element;
        var CountryName = document.getElementById("countryNameUpdate").value;
        var CountryCurrency = document.getElementById("countryCurrencyUpdate").value;
        var LegalRequirements = document.getElementById("legalRequirementsUpdate").value;
        var MinimumLeave = parseInt(document.getElementById("minimumLeaveUpdate").value);

        console.log(element);

        var CountryPosting = {
            "CountryId": CountryId,
            "CountryName": CountryName,
            "Currency": CountryCurrency,
            "LegalRequirements": LegalRequirements.split(',').map(item => item.trim()),
            "MinimumLeave": MinimumLeave
        };
        console.log(CountryPosting);
        FetchRequest('UpdateCountry', CountryPosting);
        alert('Successful')
        populateCountryTable();
    });
}
function DeleteCountry() {
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No countries selected for deletion.");
        return;
    }
}

selectedIds.forEach(element => {
    var CountryId = element;

    var CountryPosting = {
        "CountryId": CountryId,
        "CountryName": "NULL",
        "Currency": "NULL",
        "LegalRequirements": [],
        "MinimumLeave": 0
    };

    FetchRequest('DeleteCountry', CountryPosting);
    populateCountryTable();
});



// Regions BUTTONS
function addNewRegion() {
    console.log("Method Start");
    var RegionId = Math.floor(Math.random() * 1000) + 1;
    var RegionName = document.getElementById("regionName").value;
    var CountryId = document.getElementById('CountryOptions2').value;


    console.log(CountryId)
    console.log(RegionName)
    console.log(RegionId)



    var Region = {
        "RegionId": RegionId,
        "RegionName": RegionName,
        "CountryId": CountryId
    };

    console.log(Region)
    FetchRequest('InsertRegion', Region);
    alert('Successful')
    populateRegionTable();
}

function updateRegion() {
    console.log("Request Received");
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No regions selected for updating.");
        return;
    }

    selectedIds.forEach(element => {
        var RegionId = element;
        var RegionName = document.getElementById("regionNameUpdate").value;
        var CountryId = document.getElementById("CountryOptions3").value;

        console.log(element);

        var RegionPosting = {
            "RegionId": RegionId,
            "RegionName": RegionName,
            "CountryId": CountryId
        };
        console.log(RegionPosting);
        FetchRequest('UpdateRegion', RegionPosting);
        alert('Successful')
        populateRegionTable();
    });
}
function DeleteRegion() {
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No regions selected for deletion.");
        return;
    }

    selectedIds.forEach(element => {
        var RegionId = element;

        var RegionPosting = {
            "RegionId": RegionId,
            "RegionName": "NULL",
            "CountryId": "NULL"
        };

        FetchRequest('DeleteRegion', RegionPosting);
        alert('Successful')
        populateRegionTable();
    });
}



// STATIC CALLS PLEASE DONT MOVE
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