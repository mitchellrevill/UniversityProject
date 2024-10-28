const host = `${window.location.protocol}//${window.location.host}`;

fetchApiRequest('GetAllEmployees');
fetchApiRequest('GetAllJobPostings');

async function fetchApiRequest(functionName) {
    try {
        var response = await fetch(host + '/' + functionName);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        console.log(response);

        var jsonResponse = await response.json();
        console.log('apiRequest function successful:', jsonResponse);
        switch (functionName) {
            case "GetAllEmployees":
                displayEmployees(jsonResponse);
                break;
            case "GetAllJobPostings":
                displayJobs(jsonResponse);
        }

    } catch (error) {
        console.error('Failed to fetch employees:', error);
        alert('Failed to load employee data. Please check your connection.');
    }
}

function displayEmployees(employees) {
    const tbody = document.querySelector('#jsonTable tbody');

    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    tbody.innerHTML = ''; // Clear any existing rows

    employees.forEach(employee => {
        const row = document.createElement('tr');

        row.innerHTML = `
            <td>${employee.EmployeeId}</td>
            <td>${employee.FirstName}</td>
            <td>${employee.LastName}</td>
            <td>${employee.CompanyEmail}</td>
            <td>${employee.PersonalEmail}</td>
        `;
        tbody.appendChild(row);
    });
}

function displayJobs(jobs) {
    const tbody = document.querySelector('#jobPostingsTableBody');

    console.log('Jobs data:', jobs);


    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    console.log(jobs);

    tbody.innerHTML = ''; // Clear any existing rows

    jobs.forEach(job => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td><a href="#container2"><button type="button" class="submit-button" onclick="AddNewApplicant(${job.postingId})">Add Job</button></a></td>
            <td>${job.postingId}</td>
            <td>${job.Title}</td>
            <td>${job.Salary}</td>
            <td>${job.JobType}</td>
            <td>${job.Hours}</td>
            <td>${job.JobDescription}</td>
        `;

        tbody.appendChild(row);
    });
}

async function AddNewApplicant(postingId){
    post = await FetchRequestGET('GetJobPostByID')
   // HERE FUTURE ME // ADD TO METHOD HANDLER TO GET BY ID ONCE GOTTEN BY ID MAKE LOOP TO POPULATE APPLICATION PLEASE 
}



function addNewJob() {

    var postingId = Math.random().toString(36).substring(2, 9); // Generate a random ID
    var jobTitle = document.getElementById("jobTitle").value;
    var jobDescription = document.getElementById("jobDescription").value;
    var jobType = document.getElementById("jobType").value;
    var salary = document.getElementById("jobSalary").value;
    var hours = document.getElementById("jobHours").value;

    // Validate required fields
    if (!jobTitle || !jobTitle || !jobDescription || !jobType || !salary || !hours) {
        alert("You have not answered all required fields");
        return;
    }

    // Define the jobPosting object before using it
    var jobPosting = {
        "postingId": postingId,
        "Title": jobTitle,
        "JobDescription": jobDescription,
        "JobType": jobType,
        "Hours": hours,
        "Salary": salary
    };

    // Use fetch to send the job posting data
    fetch('http://localhost:8000/InsertJobPosting', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(jobPosting)
    })
        .then(response => {
            if (response.ok) {
                return response.text(); // Success message
            } else {
                throw new Error('Error inserting the job posting');
            }
        })
        .then(data => {
            alert(data); // Show success message
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Failed to insert job posting');
        });
}


function newJobContainerVisible() {
    document.getElementById("addJobForm").style.display = "block";
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