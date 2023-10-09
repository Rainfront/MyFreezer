import {Form} from "react-router-dom";
import {Label} from "@mui/icons-material";
import {Alert, Box, Button, Card, FormGroup, IconButton, Input, TextField, Typography} from "@mui/material";
import { useState } from "react";
import CloseIcon from '@mui/icons-material/Close';
import { ajaxForLogin } from "../Redux/Requests/LoginRequests";

export default function Login() {

    const [login,setLogin] = useState("");
    const [password,setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const [success, setSuccess] = useState(false);
    const handleSubmit = (event:React.FormEvent<HTMLFormElement>)=>{
        event.preventDefault();
        if(login == "" || password == ""){
            setErrorMessage("Check input vales!");
            return;
        }
        ajaxForLogin({
            "login":{
                "login":login,
                "password":password
            }
        }).subscribe({
            next: () => {setSuccess(true)},
            error: (error) => {setErrorMessage(error.toString())}
            }
        )
    }

    return (<>
        <Box display={"flex"} justifyContent="center" alignItems="center">
            <Box width={"50%"} m={"auto"}>
                <Card variant={"outlined"}>
                    <form onSubmit={(e)=> handleSubmit(e)}>
                        <FormGroup>
                            <Typography margin={"10px"}>Enter login</Typography>
                            <TextField label={"Login"}
                                       value = {login} onChange={(e)=>setLogin(e.target.value)}
                            />
                        </FormGroup>
                        <FormGroup>
                            <Typography margin={"10px"}>Enter password</Typography>
                            <TextField type={"password"} label={"Password"}
                            value = {password} onChange={(e)=>setPassword(e.target.value)}
                            />
                        </FormGroup>
                        <Button type={"submit"} variant={"outlined"}>Login</Button>
                    </form>
                    {success ? 
                        <>
                            <Alert severity={"success"}
                                   action={<Button color={"inherit"}>Go to main page</Button>}>
                                You are logged!
                            </Alert>
                        </> : <></>
                    }
                    {errorMessage !== "" ?
                        <Alert severity={"warning"}
                               action={<IconButton
                                   aria-label="close"
                                   color="inherit"
                                   size="small"
                                   onClick={() => {
                                       setErrorMessage("");
                                   }}
                               >
                                   <CloseIcon fontSize="inherit" />
                               </IconButton>}>{errorMessage}</Alert> :
                        <></>}
                </Card>
                
            </Box>
        </Box>
    </>)
}