import React from 'react';
import { Form, Input, Button } from 'antd';
import RegistrationService from '../Services/registration.service';

const registration = new RegistrationService;

type registerUser = {
    Username: string;
    Password: string;
    Email: string;
    RoleName: string;
    FirstName: string;
    LastName: string;
};

function Register(){
    const [form] = Form.useForm();

    const onFinish = (values: registerUser) => {
        registration.registerNewUser(values);
        // console.log('Received values:', values);
    };
    return (
        <Form
            form={form}
            labelCol={{ span: 4 }}
            wrapperCol={{ span: 14 }}
            layout="horizontal"
            onFinish={onFinish}
            style={{ maxWidth: 600 }}
        >
            <Form.Item label="Username" name="Username" rules={[{ required: true, message: 'Please enter your username' }]}>
            <Input />
            </Form.Item>
            <Form.Item label="Password" name="Password" rules={[{ required: true, message: 'Please enter your password' }]}>
            <Input.Password />
            </Form.Item>
            <Form.Item label="Email" name="Email" rules={[{ required: true, message: 'Please enter your email' }]}>
            <Input />
            </Form.Item>
            <Form.Item label="Role Name" name="RoleName" rules={[{ required: true, message: 'Please enter your role name' }]}>
            <Input />
            </Form.Item>
            <Form.Item label="First Name" name="FirstName" rules={[{ required: true, message: 'Please enter your first name' }]}>
            <Input />
            </Form.Item>
            <Form.Item label="Last Name" name="LastName" rules={[{ required: true, message: 'Please enter your last name' }]}>
            <Input />
            </Form.Item>
            <Form.Item wrapperCol={{ offset: 4, span: 14 }}>
            <Button type="primary" htmlType="submit">
                Submit
            </Button>
            </Form.Item>
        </Form>
    );
}
export default Register