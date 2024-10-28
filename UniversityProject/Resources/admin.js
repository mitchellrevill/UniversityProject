const host = `${window.location.protocol}//${window.location.host}`;

populateDepartmentTable();
populateCountryTable();
populateRegionTable();
populateDepartmentTable();



async function populateDepartmentTable() {
    try {
        const Departments = await FetchRequestGET('GetAllDepartments'); 
        if (!Array.isArray(Departments)) {
            throw new Error('Expected an array from FetchRequestGET.');
        }
        console.log(Departments)

        const tbody = document.getElementById('departmentTableBody'); 
        tbody.innerHTML = ''; 
     
        Departments.forEach(item => {
            console.log("Entered Loop")
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

async function populateCountryTable() {

    Countries = await FetchRequestGET('GetCountries');


   console.log(Countries)

    
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

function getSelectedIds() {
    const selectedIds = [];
    document.querySelectorAll(".dynamic-checkbox-item:checked").forEach((checkbox) => {
        selectedIds.push(checkbox.dataset.id);  // Collect selected IDs
    });
    return selectedIds;
}

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
    });
}



function addNewCountry() {
    console.log("Method Start");
    var CountryId = document.getElementById("countryId").value;
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
    });
}
async function DeleteCountry() {
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
    });

function addNewRegion() {
    console.log("Method Start");
    var RegionId = document.getElementById("regionId").value;
    var RegionName = document.getElementById("regionName").value;
    var CountryId = document.getElementById('cars').value;


    console.log(CountryId)
    console.log(RegionName)
    console.log(RegionId)



    var Region = {
        "RegionId": RegionId,
        "RegionName": RegionName,
        "CountryId": CountryId
    };

    FetchRequest('InsertRegion', Region);
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
        var CountryId = document.getElementById("countryIdUpdate").value;

        console.log(element);

        var RegionPosting = {
            "RegionId": RegionId,
            "RegionName": RegionName,
            "CountryId": CountryId
        };
        console.log(RegionPosting);
        FetchRequest('UpdateRegion', RegionPosting);
    });
}
async function DeleteRegion() {
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
    });
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
            alert(data);
        } else {
            throw new Error('Error performing operation on the Department');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation on the Department');
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
            throw new Error('Error performing operation on the Department');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation on the Department');
    }
}