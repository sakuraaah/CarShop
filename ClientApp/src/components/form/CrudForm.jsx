import React, { useState, useEffect, useContext } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import {
  Button, 
  Form, 
  Label,
  LabelFormItem,
  Loader,
  Input,
} from '../../ui';
import { 
  ButtonList,
  FormHeader,
  StyledPage, 
  StyledWrapper,
} from '../../styles/layout/form';
import useQueryApiClient from '../../utils/useQueryApiClient';
import { message } from 'antd';
import dayjs from 'dayjs';
import { UserDataContext } from '../../contexts/UserDataProvider'

export const CrudForm = ({
  form,
  url,
  apiUrl,
  name,
  parseResponseToForm,
  parseFormToSubmit,
  disabled,
  setDisabled,
  children
}) => {
  const { id } = useParams()
  const navigate = useNavigate()
  const location = useLocation()

  const [labelPrefix, setLabelPrefix] = useState('')
  const [seller, setSeller] = useState('')
  const [status, setStatus] = useState('')
  const [adminStatus, setAdminStatus] = useState('')
  const [adminComment, setAdminComment] = useState('')
  const [created, setCreated] = useState('')
  const [availableStatusTransitions, setAvailableStatusTransitions] = useState([])

  const { data: userData } = useContext(UserDataContext)

  useEffect(() => {
    if (id) {
      getPost();
    } else {
      setLabelPrefix('Create')
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const goBack = () => {
    if (location.state?.fromNew) {
      navigate(-2)
    } else {
      navigate(-1)
    }
  }

  const onSubmit = async (newStatus = null) => {
    try {
      const values = await form.validateFields()

      if (!id) {
        let options = values
        options = parseFormToSubmit && parseFormToSubmit(options)
        createPost(options)
      } else {
        if (newStatus) {
          updatePostStatus({
            status: newStatus,
            comment: form.getFieldValue('comment')
          })
        } else {
          let options = values
          options = parseFormToSubmit && parseFormToSubmit(options)
          updatePost(options)
        }
      }
    } catch (errorInfo) {
      form.scrollToField(errorInfo.errorFields[0]?.name, { behavior: 'smooth', block: 'center', scrollMode: 'if-needed' })
    }
  }

  const { refetch: getPost, isLoading: getLoading } = useQueryApiClient({
    request: {
      url: `${apiUrl}/${id}`,
      method: 'GET',
      disableOnMount: true,
    },
    onSuccess: (response) => {
      let formData = response.data
      formData = parseResponseToForm && parseResponseToForm(formData)
      form.setFieldsValue(formData)

      if (response.data?.status === 'Draft') {
        setLabelPrefix('Edit')
      } else {
        setLabelPrefix('View')
        setDisabled(true)
      }

      setStatus(response.data?.status)
      setSeller(response.data?.user?.userName)
      setAdminStatus(response.data?.adminStatus)
      setAdminComment(response.data?.adminComment)
      setCreated(dayjs(response.data?.created).format('DD-MM-YYYY'))
      setAvailableStatusTransitions(response.data?.availableStatusTransitions)
    },
    onError: () => {
      goBack()
    }
  });

  const { appendData: createPost, isLoading: createLoading } = useQueryApiClient({
    request: {
      url: apiUrl,
      method: 'POST'
    },
    onSuccess: (response) => {
      message.success(`${name} is succesfully created`)
      navigate(`/${url}/${response.data?.id}`, { state: { fromNew: true } })
    }
  });

  const { appendData: updatePost, isLoading: updateLoading } = useQueryApiClient({
    request: {
      url: `${apiUrl}/${id}`,
      method: 'PATCH'
    },
    onSuccess: () => {
      message.success(`${name} is succesfully updated`)
    }
  });

  const { appendData: updatePostStatus, isLoading: updateStatusLoading } = useQueryApiClient({
    request: {
      url: `${apiUrl}/${id}/status`,
      method: 'PATCH'
    },
    onSuccess: (response) => {
      message.success(`Status for ${name} is succesfully updated`)

      setLabelPrefix('View')
      setDisabled(true)

      setStatus(response.data?.status)
      setAdminStatus(response.data?.adminStatus)
      setAdminComment(response.data?.adminComment)
      setAvailableStatusTransitions(response.data?.availableStatusTransitions)
    }
  });

  return (
    <StyledPage>
      <Form 
        form={form}
        disabled={disabled}
      >
        <FormHeader>
          <Label 
            label={`${labelPrefix} ${name}`} 
            extraBold 
          />

          {seller &&
            <LabelFormItem 
              label={'Seller'} 
              labelValue={seller}
            />
          }

          {status &&
            <LabelFormItem 
              label={'Status'} 
              labelValue={status}
            />
          }

          {created &&
            <LabelFormItem 
              label={'Created'} 
              labelValue={created}
            />
          }

          {adminStatus &&
            <LabelFormItem 
              label={'Admin status'} 
              labelValue={adminStatus}
            />
          }

          {adminComment && 
            <LabelFormItem 
              label={'Admin comment'} 
              labelValue={adminComment}
            />
          }
        </FormHeader>

        <Loader loading={createLoading || getLoading || updateLoading || updateStatusLoading} >

          {children}

          <StyledWrapper>
            <ButtonList>

              {(!id || seller == userData?.userName) && (
                <>
                  <Button 
                    htmlType="submit" 
                    onClick={() => onSubmit()} 
                    type="primary" 
                    label={'Save'} 
                  />

                  {availableStatusTransitions.map((status) => {
                    let statusName;

                    switch (status.name) {
                      case 'Submitted':
                        statusName = 'Submit'
                        break

                      case 'Cancelled':
                        statusName = 'Cancel'
                        break

                      default:
                        statusName = status.name
                        break
                    }

                    return (
                      <Button 
                        htmlType="submit" 
                        onClick={() => onSubmit(status.name)} 
                        label={statusName} 
                        disabled={false}
                        danger={status.name === 'Cancelled'}
                      />
                    )
                  })}
                </>
              )}

              {(userData?.role == 'Admin') && (
                <>
                  <Input
                    name="comment"
                    label={'Provide a comment to seller'}
                    disabled={status != 'Submitted'}
                  />
                  <Button 
                    onClick={() => onSubmit('Confirmed')} 
                    label={'Approve'} 
                    disabled={status != 'Submitted'}
                    type="primary"
                  />
                  <Button 
                    onClick={() => onSubmit('Blocked')} 
                    label={'Block'} 
                    disabled={status != 'Submitted'}
                    danger
                  />
                </>
              )}

              <Button 
                onClick={goBack} 
                label={'Return'}
                disabled={false}
              />
            </ButtonList>
          </StyledWrapper>
        </Loader>
      </Form>
    </StyledPage>
  )
}
