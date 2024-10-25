//--------------//
//Employees page//
//--------------//

function loadEmployeesFromStorage() {

    var employeesData = localStorage.getItem('employeesData');

    if (employeesData) {
        var employees = JSON.parse(employeesData);
        displayEmployees(employees); // Call the display function with the parsed data
    } else {
        console.error('No employee data found in localStorage');
        alert('Failed to load employee data. Please try again.');
    }
}
function displayEmployees(employees) {
    var tbody = document.querySelector('#employeeJsonTable tbody');

    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    tbody.innerHTML = ''; // Clear any existing rows

    employees.forEach(employee => {
        var tr = document.createElement('tr');

        // Populate the table row with employee data
        var empIdTd = document.createElement('td');
        empIdTd.innerText = employee.EmployeeId;
        tr.appendChild(empIdTd);

        var firstNameTd = document.createElement('td');
        firstNameTd.innerText = employee.FirstName;
        tr.appendChild(firstNameTd);

        var lastNameTd = document.createElement('td');
        lastNameTd.innerText = employee.LastName;
        tr.appendChild(lastNameTd);

        var companyEmailTd = document.createElement('td');
        companyEmailTd.innerText = employee.CompanyEmail;
        tr.appendChild(companyEmailTd);

        var personalEmailTd = document.createElement('td');
        personalEmailTd.innerText = employee.PersonalEmail;
        tr.appendChild(personalEmailTd);

        tbody.appendChild(tr);
    });
}
// Trigger loading of employees and jobs
window.addEventListener('load', loadEmployeesFromStorage);



//---------//
//Jobs page//
//---------//

function loadJobsFromStorage() {

    var jobsData = localStorage.getItem('jobPostingData');

    if (jobsData) {
        var jobs = JSON.parse(jobsData);
        displayJobs(jobs); // Call the display function with the parsed data
    } else {
        console.error('No jobs data found in localStorage');
        alert('Failed to load employee data. Please try again.');
    }
}

function displayJobs(jobs) {
    var tbody = document.querySelector('#job-results tbody');

    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    tbody.innerHTML = ''; // Clear any existing rows

    jobs.forEach(job => {
        var tr = document.createElement('tr');

        var postingIdTd = document.createElement('td');
        postingIdTd.innerText = job.postingId;
        tr.appendChild(postingIdTd);

        var jobTitleTd = document.createElement('td');
        jobTitleTd.innerText = job.Title;
        tr.appendChild(jobTitleTd);

        var jobDescriptionTd = document.createElement('td');
        jobDescriptionTd.innerText = job.jobdesc;
        tr.appendChild(jobDescriptionTd);

        var jobtypeTd = document.createElement('td');
        jobtypeTd.innerText = job.jobtype;
        tr.appendChild(jobtypeTd);

        var hoursTd = document.createElement('td');
        hoursTd.innerText = job.hours;
        tr.appendChild(hoursTd);

        var salaryTd = document.createElement('td');
        salaryTd.innerText = job.salary;
        tr.appendChild(salaryTd);

        var actionTd = document.createElement('td');
        actionTd.innerHTML = '<button>Apply</button>';
        tr.appendChild(actionTd);

        tbody.appendChild(tr);
    });
}

window.addEventListener('load', loadJobsFromStorage);
function addNewJob() {

    var postingId = Math.random().toString(36).substring(2, 9); // Generate a random ID
    var jobTitle = document.getElementById("jobTitle").value;
    var jobDescription = document.getElementById("jobDescription").value;
    var jobType = document.getElementById("jobType").value;
    var hours = document.getElementById("hours").value;
    var salary = document.getElementById("salary").value;

    // Validate required fields
    if (!jobTitle || !salary || !jobDescription || !location) {
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



