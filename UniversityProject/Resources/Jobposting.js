const host = `${window.location.protocol}//${window.location.host}`;

FetchRequestGET('GetAllEmployees');
FetchRequestGET('GetAllJobPostings');



populatePostingTable()

async function AddNewApplicant(postingId) {
    const post = await FetchRequestGET(`GetJobPostByID/${postingId}`);
    console.log('Selected job post:', post);
}



async function populatePostingTable() {
    try {
        const Departments = await FetchRequestGET('GetAllJobPostings');
        const locations = await FetchRequestGET("GetLocations");

        if (!Array.isArray(Departments) || !Array.isArray(locations)) {
            throw new Error('Expected arrays from FetchRequestGET.');
        }

        const tbody = document.getElementById('jobPostingsTableBody');
        tbody.innerHTML = '';

        if (Departments.length === 0) {
            tbody.innerHTML = '<tr><td colspan="8">No job postings available.</td></tr>';
            return;
        }

        
        for (const item of Departments) {
            const location = locations.find(loc => loc.LocationId === item.locationId);
            const locationName = location ? location.LocationName : 'Unknown Location'; 

            
            var Location = {
                "LocationId": item.LocationId,
                "LocationName": "NULL",
                "RegionId": 1,
                "CountryId": 1,
                "Latitude": 1,
                "Longitude": 1
            };

            
            const locationObject = await FetchRequest("GetLocationById", Location);

            
            const row = document.createElement('tr');
            row.innerHTML = `   
                <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.postingId}" data-name="${item.jobTitle}" data-description="${item.jobDescription}"></td>
                <td><a href="#apply"><button style="background-color: #6290c8; padding: 10px; border-radius: 15px; color: white;">Apply</button></a></td>
                <td>${item.postingId}</td>
                <td>${item.Title}</td>
                <td>${item.Salary}</td>
                <td>${item.JobDescription}</td>
                <td>${item.JobType}</td>
                <td>${item.Hours}</td>
                <td>${locationObject ? locationObject.LocationName : locationName}</td> 
            `;

            tbody.appendChild(row);
        }
    } catch (error) {
        console.error('Error populating job postings table:', error);
    }
}




function addNewJob() {
    var postingId = Math.random().toString(36).substring(2, 9); 
    var jobTitle = document.getElementById("jobTitle").value;
    var jobDescription = document.getElementById("jobDescription").value;
    var jobType = document.getElementById("jobType").value;
    var salary = document.getElementById("jobSalary").value;
    var hours = document.getElementById("jobHours").value;
    var location = document.getElementById("LocationsOptions").value;

    // Validate required fields
    if (!jobTitle || !jobDescription || !jobType || !salary || !hours || !location) {
        alert("You have not answered all required fields");
        return;
    } else {
       
        var jobPosting = {
            "postingId": postingId,
            "Title": jobTitle,
            "Salary": salary,
            "JobDescription": jobDescription,
            "JobType": jobType,
            "Hours": hours,
            "LocationId": locationObject.locationName
        };
        FetchRequest("InsertJobPosting", jobPosting)
        populateDepartmentTable() 
    }
}



function populateLocationsOptions() {
    FetchRequestGET('GetLocations')
        .then(Countries => {
            const selectElements = document.querySelectorAll('.Locations-options');

            console.log(selectElements);
            selectElements.forEach(select => {
                select.innerHTML = '';

                Countries.forEach(item => {
                    const option = document.createElement('option');
                    option.value = item.LocationId;
                    option.textContent = item.LocationName;

                    select.appendChild(option);
                });
            });
        })
        .catch(error => {
            console.error('Error fetching locations:', error);
        });
}


populateLocationsOptions()

async function FetchRequest(uri, model) {
    console.log("Req sent");


    const token = localStorage.getItem("authToken");


    let headers = {
        'Content-Type': 'application/json'
    };


    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    try {
        const url = host + '/' + uri;
        console.log('URL:', url);
        console.log('Headers:', headers);
        console.log('Body:', JSON.stringify(model));

        const response = await fetch(url, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(model)
        });

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            const errorText = await response.text();
            throw new Error(`Error ${response.status}: ${response.statusText}. ${errorText}`);
        }
    } catch (error) {
        console.error('Error:', error);
        return null;
    }
}
async function FetchRequestGET(uri) {
    try {
        const token = localStorage.getItem("authToken");

        let headers = {
            'Content-Type': 'application/json'
        };

        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        // Make the GET request
        const response = await fetch(host + '/' + uri, {
            method: 'GET',
            headers: headers
        });

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            throw new Error('Error performing operation');
        }
    } catch (error) {
        console.error('Error:', error);
        
    }
}


