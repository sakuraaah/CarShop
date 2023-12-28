import React, { useState, useEffect, useContext } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import {
  Button, 
  DatePicker,
  Form, 
  Label,
  LabelFormItem,
  Loader,
  Input,
  Modal,
  Popconfirm
} from '../../ui';
import { 
  ButtonList,
  FormHeader,
  StyledPage, 
  StyledWrapper,
} from '../../styles/layout/form';
import useQueryApiClient from '../../utils/useQueryApiClient';
import { message, Form as AntdForm } from 'antd';
import dayjs from 'dayjs';
import { UserDataContext } from '../../contexts/UserDataProvider'

export const CrudForm = ({
  form,
  url,
  apiUrl,
  name,
  type,
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
  const [rentCategory, setRentCategory] = useState()
  const [status, setStatus] = useState('')
  const [adminStatus, setAdminStatus] = useState('')
  const [adminComment, setAdminComment] = useState('')
  const [created, setCreated] = useState('')
  const [availableStatusTransitions, setAvailableStatusTransitions] = useState([])

  const [isRentModalOpened, setIsRentModalOpened] = useState(false)

  const { data: userData } = useContext(UserDataContext)

  const [rentForm] = AntdForm.useForm()

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
      setRentCategory(response.data?.rentCategory?.name)
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
    onSuccess: (response) => {
      setRentCategory(response.data?.rentCategory?.name)
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

  const { refetch: buyVehicle, isLoading: buyVehicleLoading } = useQueryApiClient({
    request: {
      url: `${apiUrl}/${id}/buy`,
      method: 'POST'
    },
    onSuccess: (response) => {
      const vehicleName = (response.data?.mark.name !== 'Other') ? `${response.data?.mark.name} ${response.data?.model}` : response.data?.model
      message.success(`Vehicle ${vehicleName} is succesfully bought`)

      setStatus(response.data?.status)

      navigate('/profile/bought-vehicles')
    }
  });

  const { refetch: shareVehicle, isLoading: shareVehicleLoading } = useQueryApiClient({
    request: {
      url: `${apiUrl}/${id}/rent-carsharing`,
      method: 'POST'
    },
    onSuccess: (response) => {
      message.success(`You succesfully started sharing ${response.data?.rentItem}`)

      setStatus('Busy')

      navigate('/profile/rent-orders')
    }
  });

  const { appendData: rentVehicle, isLoading: rentVehicleLoading } = useQueryApiClient({
    request: {
      url: `${apiUrl}/${id}/rent-daily`,
      method: 'POST'
    },
    onSuccess: (response) => {
      message.success(`You succesfully rented ${response.data?.rentItem} till ${dayjs(response.data?.endTime).format('DD-MM-YYYY')}`)

      setStatus('Busy')

      navigate('/profile/rent-orders')
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

          {rentCategory &&
            <LabelFormItem 
              label={'Rental category'} 
              labelValue={rentCategory}
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

        <Loader loading={createLoading || getLoading || updateLoading || updateStatusLoading || buyVehicleLoading || shareVehicleLoading || rentVehicleLoading} >

          {children}

          <StyledWrapper>
            <ButtonList>

              {(userData?.role == 'Buyer') && (
                <>
                  {(type == 'Product' && status == 'Submitted') && (
                    <Popconfirm
                      title="Buy a vehicle"
                      description="Are you sure you want to proceed?"
                      onConfirm={buyVehicle}
                      okButtonProps={{
                        disabled: false
                      }}
                      cancelButtonProps={{
                        disabled: false
                      }}
                    >
                      <Button 
                        label={'Buy'}
                        type="primary"
                        disabled={false}
                      />
                    </Popconfirm>
                  )}

                  {(type == 'Rental' && rentCategory == 'Carsharing') && (
                    <Popconfirm
                      title="Start sharing a car"
                      description="Are you sure you want to proceed?"
                      onConfirm={shareVehicle}
                      okButtonProps={{
                        disabled: (status != 'Submitted')
                      }}
                      cancelButtonProps={{
                        disabled: (status != 'Submitted')
                      }}
                    >
                      <Button 
                        label={'Rent'}
                        type="primary"
                        disabled={status != 'Submitted'}
                      />
                    </Popconfirm>
                  )}

                  {(type == 'Rental' && rentCategory == 'Daily') && (
                    <>
                      <Button 
                        label={'Rent'}
                        type="primary"
                        onClick={() => setIsRentModalOpened(true)} 
                        disabled={status != 'Submitted'}
                      />
                      <Modal 
                        title="Rent a vehicle" 
                        open={isRentModalOpened} 
                        onOk={async () => {
                          try {
                            const fields = await rentForm.validateFields()
                            rentVehicle({
                              endDate: dayjs(fields.endDate).format('YYYY-MM-DD')
                            })
                            
                            rentForm.resetFields()
                            setIsRentModalOpened(false)
                          } catch (e) {
                            console.error(e)
                          }
                        }} 
                        onCancel={() => {
                          rentForm.resetFields()
                          setIsRentModalOpened(false)
                        }}
                      >
                        <Form form={rentForm}>
                          <DatePicker
                            name="endDate"
                            label={'Date'}
                            disabled={false}
                            disabledDate={(date) => date < dayjs().subtract(1, 'day')}
                            rules={[{ required: true }]}
                          />
                        </Form>
                      </Modal>
                    </>
                  )}
                </>
              )}

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
                      <Popconfirm
                        title="Change status"
                        description="Are you sure you want to change status?"
                        onConfirm={() => onSubmit(status.name)}
                        okButtonProps={{
                          disabled: false
                        }}
                        cancelButtonProps={{
                          disabled: false
                        }}
                      >
                        <Button 
                          htmlType="submit"
                          label={statusName} 
                          disabled={false}
                          danger={status.name === 'Cancelled'}
                        />
                      </Popconfirm>
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

                  <Popconfirm
                    title="Change status"
                    description="Are you sure you want to change status?"
                    onConfirm={() => onSubmit('Confirmed')}
                    okButtonProps={{
                      disabled: (status != 'Submitted')
                    }}
                    cancelButtonProps={{
                      disabled: (status != 'Submitted')
                    }}
                  >
                    <Button 
                      label={'Approve'} 
                      disabled={status != 'Submitted'}
                      type="primary"
                    />
                  </Popconfirm>

                  <Popconfirm
                    title="Change status"
                    description="Are you sure you want to change status?"
                    onConfirm={() => onSubmit('Blocked')}
                    okButtonProps={{
                      disabled: (status != 'Submitted')
                    }}
                    cancelButtonProps={{
                      disabled: (status != 'Submitted')
                    }}
                  >
                    <Button 
                      label={'Block'} 
                      disabled={status != 'Submitted'}
                      danger
                    />
                  </Popconfirm>
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
