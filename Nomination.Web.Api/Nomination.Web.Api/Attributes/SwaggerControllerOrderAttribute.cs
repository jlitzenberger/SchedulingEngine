﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nomination.Web.Api.Attributes
{
    /// <summary>
    /// Annotates a controller with a Swagger sorting order that is used when generating the Swagger documentation to
    /// order the controllers in a specific desired order.
    /// </summary>
    public class SwaggerControllerOrderAttribute : Attribute
    {
        /// <summary>
        /// Gets the sorting order of the controller.
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerControllerOrderAttribute"/> class.
        /// </summary>
        /// <param name="order">Sets the sorting order of the controller.</param>
        public SwaggerControllerOrderAttribute(int order)
        {
            Order = order;
        }
    }
}