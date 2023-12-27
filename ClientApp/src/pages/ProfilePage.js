import React, { useState, useEffect, useContext } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { Form as AntdForm, message } from 'antd';
import {
  Button,
  Form,
  ImageUpload,
  Input,
  Label,
  Loader,
  SideBySide,
} from '../ui';
import { 
  BorderBottom,
  ButtonList,
  FormHeader,
  StyledPage, 
  StyledWrapper,
} from '../styles/layout/form';
import useQueryApiClient from '../utils/useQueryApiClient';
import { UserDataContext } from '../contexts/UserDataProvider'

export const ProfilePage = () => {
  const [form] = AntdForm.useForm();

  const [isEditPage, setIsEditPage] = useState(false)
  const [imageUrl, setImageUrl] = useState()

  const { data: userData, refetch } = useContext(UserDataContext)

  const navigate = useNavigate()
  const location = useLocation();

  useEffect(() => {
    setIsEditPage(location.pathname.endsWith('/edit'))
  }, [location])

  useEffect(() => {
    setImageUrl(userData?.imgSrc)

    form.setFieldsValue({
      imgSrc: userData?.imgSrc,
      firstName: userData?.firstName,
      lastName: userData?.lastName,
    })
  }, [userData])

  const { appendData: updateUser, isLoading } = useQueryApiClient({
    request: {
      url: `api/user-data`,
      method: 'PATCH'
    },
    onSuccess: () => {
      message.success('User info is succesfully updated')
      refetch()
      navigate('/profile')
    }
  });

  const goBack = () => {
    setImageUrl(userData?.imgSrc)

    form.setFieldsValue({
      imgSrc: userData?.imgSrc,
      firstName: userData?.firstName,
      lastName: userData?.lastName,
    })

    navigate(-1)
  }

  return (
    <StyledPage>
      <FormHeader>
        <Label 
          label={'Profile'} 
          extraBold 
        />
      </FormHeader>
      <Form 
        form={form}
        disabled={!isEditPage}
        onFinish={(values) => updateUser(values)}
      >
        <Loader loading={!userData || isLoading} >

          <StyledWrapper>
            <Label label={'User info:'} extraBold />

            <BorderBottom />

            <ImageUpload
              form={form}
              imageUrl={imageUrl}
              setImageUrl={setImageUrl}
              name="imgSrc"
              disabled={!isEditPage}
            />

            <BorderBottom />

            <SideBySide
              left={
                <Input
                  name="firstName"
                  label={'First Name'}
                  rules={[{ required: true }]}
                />
              }
            />

            <SideBySide
              left={
                <Input
                  name="lastName"
                  label={'Last Name'}
                  rules={[{ required: true }]}
                />
              }
            />

          </StyledWrapper>

        </Loader>

        <StyledWrapper>
          <ButtonList>
            {isEditPage ? (
              <>
                <Button 
                  htmlType="submit" 
                  type="primary" 
                  label={'Save changes'} 
                  disabled={false}
                />
                <Button 
                  onClick={goBack} 
                  label={'Return'}
                  disabled={false}
                />
              </>
            ) : (
              <Button 
                type="primary" 
                onClick={() => navigate('edit')} 
                label={'Edit'}
                disabled={false}
              />
            )}
          </ButtonList>
        </StyledWrapper>
      </Form>
    </StyledPage>
  )
}
