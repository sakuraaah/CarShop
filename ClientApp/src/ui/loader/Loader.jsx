import React from 'react';
import { Icon } from '../../ui';
import styled from 'styled-components';
import { ReactComponent as LoaderIcon } from '../../assets/LoaderIcon2.svg' 
import FadeIn from 'react-fade-in';

export const Loader = ({
  loading,
  children
}) => {
  const LoaderIconContainer = styled.div`
    display: flex;
    justify-content: center;
    padding: 150px;
  `;

  const IconWrapper = styled.div`
    position: relative;
    width: 100%;
    height: 100%;

    max-width: 140px;
    max-height: 140px;

    svg {
      &.loader-icon {
        width: 100%;
        height: 100%;
      }

      &.center-icon {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%,-50%);

        width: 35%;
        height: 35%;
      }
    }
  `;

  return loading ? (
    <LoaderIconContainer>
      <IconWrapper>
        <LoaderIcon className="loader-icon" />
        <Icon className="center-icon" style={{ color: "#b1b8c0" }} />
      </IconWrapper>
    </LoaderIconContainer>
  ) : (
    <FadeIn>
      {children}
    </FadeIn>
  );
};
