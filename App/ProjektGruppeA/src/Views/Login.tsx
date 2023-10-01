import AuthService from '../Services/auth.service.ts';
import { Button, Form, Input, Card} from 'antd';


const auth = new AuthService;
type userCreds = {
    username: string,
    password: string
}

function onFinish(creds: userCreds){ 
    debugger //eslint-disable-line
    
    auth.authorize(creds.username, creds.password);
    console.log('Success:', creds);
  }
  
function onFinishFailed(errorInfo: any){
    console.log('Failed:', errorInfo);
  }

function Login(){

    return (
        <>
            <Card title="Login" style={{ maxWidth: 600, left: '40%', right: '50%'}}>
                <Form
                style={{textAlign: 'center'}}
                 labelCol={{ span: 8 }}
                 wrapperCol={{ span: 16 }}
                 autoComplete='on'
                 onFinish={onFinish}
                 onFinishFailed={onFinishFailed}>
                    <Form.Item<userCreds>
                        label="Benutzername" 
                        name="username" 
                        rules={[{required: true, message: 'Bitte geben Sie einen Benutzernamen an!'}]}>
                        <Input/>
                    </Form.Item>
                    <Form.Item<userCreds>
                        label="Passwort" 
                        name="password" 
                        rules={[{required: true, message: 'Bitte geben Sie ein Passwort an!'}]}>
                        <Input type='password'/>
                    </Form.Item>
                    <Form.Item wrapperCol={{offset: 8, span: 16}}>
                        <Button type='primary' htmlType='submit'>
                            Anmelden
                        </Button>
                    </Form.Item>
                </Form>
            </Card>
        </>
    )
}
export default Login