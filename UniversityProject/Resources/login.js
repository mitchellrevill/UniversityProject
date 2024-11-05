async function Login() {
    const post = await FetchRequestGET(`GetJobPostByID/${postingId}`);
    console.log('Selected job post:', post);
}